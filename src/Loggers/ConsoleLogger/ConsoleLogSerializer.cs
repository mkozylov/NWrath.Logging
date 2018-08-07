using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Reflection.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public class ConsoleLogSerializer
        : IConsoleLogSerializer, ILogSerializer
    {
        public const string DefaultOutputTemplate = "{Timestamp} [{Level}] {Message}{ExNewLine}{Exception}";

        public string OutputTemplate
        {
            get { return _outputTemplate; }
            set { _outputTemplate = SetNewOutputTemplate(value); }
        }

        public ITokenFormatStore Formats
        {
            get { return _formats; }
            set { _formats = SetNewFormats(value); }
        }

        public ITokenConsoleColorStore Colors
        {
            get { return _colors; }
            set { _colors = SetNewColors(value); }
        }

        private string _outputTemplate = DefaultOutputTemplate;
        private ITokenFormatStore _formats = new TokenFormatStore();
        private ITokenConsoleColorStore _colors = new TokenConsoleColorStore();
        private Lazy<IStringLogSerializer> _serializeFunc;
        private ITokenParser _parser = new TokenParser();
        private static PropertyInfo[] _logProps = typeof(LogMessage).GetProperties();

        public ConsoleLogSerializer()
        {
            SetNewFormats(_formats);
            SetNewColors(_colors);
        }

        public string Serialize(LogMessage log)
        {
            return _serializeFunc.Value.Serialize(log);
        }

        object ILogSerializer.Serialize(LogMessage log)
        {
            return Serialize(log);
        }

        private IStringLogSerializer BuildSerializer()
        {
            var consoleType = typeof(Console);
            var tokens = _parser.Parse(OutputTemplate);
            var logExpr = Expression.Parameter(typeof(LogMessage), "log");
            var blocks = new List<Expression>();
            var strVars = new List<ParameterExpression>();
            var logProp = default(PropertyInfo);

            var tryGetExtraMI = typeof(CollectionsExtensions)
                                   .GetStaticGenericMethod(
                                       nameof(CollectionsExtensions.TryGet),
                                       genericParamCount: 2,
                                       argsCount: 3
                                   )
                                   .MakeGenericMethod(typeof(string), typeof(string));

            for (int i = 0; i < tokens.Length; i++)
            {
                var t = tokens[i];
                var valExpr = default(Expression);
                var colorExpr = default(Expression);

                if (t.IsString)
                {
                    valExpr = Expression.Constant(t.Value);
                }
                else
                {
                    if (Formats.ContainsKey(t.Value))
                    {
                        var callFormatExpr = Expression.Constant(Formats[t.Value]);

                        valExpr = Expression.Invoke(callFormatExpr, logExpr);
                    }
                    else if ((logProp = _logProps.FirstOrDefault(x => x.Name == t.Value)) != null)
                    {
                        var msgPropExpr = Expression.Property(logExpr, logProp);

                        valExpr = Expression.Call(msgPropExpr, nameof(object.ToString), null);
                    }
                    else
                    {
                        var extraPropExpr = Expression.Property(logExpr, nameof(LogMessage.Extra));

                        var callGetExtra = Expression.Call(tryGetExtraMI, extraPropExpr, Expression.Constant(t.Value), Expression.Constant(t.Key));

                        valExpr = callGetExtra;
                    }

                    if (Colors.ContainsKey(t.Value))
                    {
                        var callTokenColor = Expression.Constant(Colors[t.Value]);

                        colorExpr = Expression.Invoke(callTokenColor, logExpr);
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

            blocks.Add(
                Expression.Call(consoleType, nameof(Console.ResetColor), null)
                );

            blocks.Add(
                Expression.Call(consoleType, nameof(Console.WriteLine), null)
                );

            blocks.Add(
                Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string[]) }),
                    Expression.NewArrayInit(typeof(string), strVars)
                    )
                );

            var body = Expression.Block(strVars, blocks);

            var writerLambda = Expression.Lambda<Func<LogMessage, string>>(body, logExpr);

            return new LambdaLogSerializer(writerLambda.Compile());
        }

        private string SetNewOutputTemplate(string newOutputFormat)
        {
            SetSerializerFunc();

            return newOutputFormat;
        }

        private ITokenFormatStore SetNewFormats(ITokenFormatStore newFormats)
        {
            _serializeFunc = new Lazy<IStringLogSerializer>(BuildSerializer);

            _formats.Updated -= SetSerializerFunc;

            newFormats.Updated -= SetSerializerFunc;
            newFormats.Updated += SetSerializerFunc;

            return newFormats;
        }

        private ITokenConsoleColorStore SetNewColors(ITokenConsoleColorStore newColors)
        {
            _serializeFunc = new Lazy<IStringLogSerializer>(BuildSerializer);

            _formats.Updated -= SetSerializerFunc;

            newColors.Updated -= SetSerializerFunc;
            newColors.Updated += SetSerializerFunc;

            return newColors;
        }

        private void SetSerializerFunc(object sender = null, EventArgs e = null)
        {
            _serializeFunc = new Lazy<IStringLogSerializer>(BuildSerializer);
        }
    }
}