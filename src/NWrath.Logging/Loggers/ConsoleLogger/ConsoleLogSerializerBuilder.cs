using FastExpressionCompiler;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Formatting;
using NWrath.Synergy.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NWrath.Logging
{
    public class ConsoleLogSerializerBuilder
        : StringSerializerBuilderBase<LogRecord, ConsoleLogSerializerBuilder, RecordFormatStore, IConsoleLogSerializer>
    {
        public const string DefaultOutputTemplate = "{Timestamp} [{Level}] {Message}{exnl}{Exception}";
        public static IConsoleLogSerializer DefaultSerializer { get; } = BuildDefaultSerializer();

        public override string OutputTemplate { get; set; } = DefaultOutputTemplate;

        public RecordConsoleColorStore Colors { get; set; } = new RecordConsoleColorStore();

        public virtual ConsoleLogSerializerBuilder UseColors(Action<RecordConsoleColorStore> colorsApply)
        {
            Colors = new RecordConsoleColorStore();

            colorsApply(Colors);

            return this;
        }

        public virtual ConsoleLogSerializerBuilder UseColors(RecordConsoleColorStore colors)
        {
            Colors = colors;

            return this;
        }

        public override Func<LogRecord, string> BuildLambda()
        {
            OutputTemplate = OutputTemplate ?? DefaultOutputTemplate;
            Formats = Formats ?? new RecordFormatStore();
            Colors = Colors ?? new RecordConsoleColorStore();
            PropertyFilter = PropertyFilter ?? (p => true);

            var consoleType = typeof(Console);
            var instanceExpr = Expression.Parameter(typeof(LogRecord), "instance");
            var blocks = new List<Expression>();
            var strVars = new List<ParameterExpression>();

            if (OutputTemplate.IsEmpty())
            {
                blocks.Add(
                    Expression.Assign(
                        Expression.Property(null, consoleType, nameof(Console.ForegroundColor)),
                        Expression.Constant(ConsoleColor.White)
                        )
                    );

                var valExpr = Expression.Call(instanceExpr, nameof(ToString), null);

                blocks.Add(
                   Expression.Call(consoleType.GetMethod(nameof(Console.Write), new[] { typeof(string) }), valExpr)
                   );
            }
            else
            {
                var tokens = parser.Parse(OutputTemplate);
                var propertyInfo = default(PropertyInfo);
                var filteredProps = props.Where(PropertyFilter)
                                         .ToArray();

                for (int i = 0; i < tokens.Length; i++)
                {
                    var t = tokens[i];
                    var valExpr = default(Expression);
                    var colorExpr = default(Expression);

                    if (t.IsLiteral)
                    {
                        valExpr = Expression.Constant(t.Value);
                    }
                    else
                    {
                        if (Formats.ContainsKey(t.Value))
                        {
                            var callFormatExpr = Expression.Constant(Formats[t.Value]);

                            valExpr = Expression.Invoke(callFormatExpr, instanceExpr);
                        }
                        else if ((propertyInfo = filteredProps.FirstOrDefault(x => x.Name.Equals(t.Value, StringComparison.OrdinalIgnoreCase))) != null)
                        {
                            var msgPropExpr = Expression.Property(instanceExpr, propertyInfo);

                            valExpr = propertyInfo.PropertyType == typeof(string)
                                       ? (Expression)msgPropExpr
                                       : Expression.Call(msgPropExpr, nameof(object.ToString), null);
                        }
                        else
                        {
                            valExpr = Expression.Constant(t.Key);
                        }

                        if (Colors.ContainsKey(t.Value))
                        {
                            var callTokenColor = Expression.Constant(Colors[t.Value]);

                            colorExpr = Expression.Invoke(callTokenColor, instanceExpr);
                        }
                    }

                    colorExpr = colorExpr ?? Expression.Constant(ConsoleColor.White);

                    blocks.Add(
                        Expression.Assign(
                            Expression.Property(null, consoleType, nameof(Console.ForegroundColor)),
                            colorExpr
                            )
                        );

                    var valVar = Expression.Parameter(typeof(string), $"tokenVar{i}");

                    strVars.Add(valVar);

                    blocks.Add(
                        Expression.Assign(valVar, valExpr)
                        );

                    blocks.Add(
                       Expression.Call(consoleType.GetMethod(nameof(Console.Write), new[] { typeof(string) }), valVar)
                       );
                }
            }

            blocks.Add(
                Expression.Call(consoleType, nameof(Console.ResetColor), null)
                );

            blocks.Add(
                Expression.Call(consoleType, nameof(Console.WriteLine), null)
                );

            blocks.Add(
                ExpressionsHelper.BuildStringConcat(strVars.ToArray())
                );

            var body = Expression.Block(strVars, blocks);
            var lambda = Expression.Lambda<Func<LogRecord, string>>(body, instanceExpr);
            var serializerFunc = lambda.CompileFast();

            return serializerFunc;
        }

        public override IConsoleLogSerializer BuildSerializer()
        {
            var serializerFunc = BuildLambda();

            return new LambdaConsoleLogSerializer(serializerFunc);
        }

        private static IConsoleLogSerializer BuildDefaultSerializer()
        {
            return new ConsoleLogSerializerBuilder()
                .UseOutputTemplate(DefaultOutputTemplate)
                .BuildSerializer();
        }
    }
}