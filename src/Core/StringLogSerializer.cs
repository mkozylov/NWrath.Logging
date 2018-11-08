using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NWrath.Synergy.Reflection.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using FastExpressionCompiler;

namespace NWrath.Logging
{
    public class StringLogSerializer
        : IStringLogSerializer, ILogSerializer
    {
        public const string DefaultOutputTemplate = "{Timestamp} [{Level}] {Message}{ExNewLine}{Exception}";

        public string OutputTemplate
        {
            get { return _outputTemplate; }
            set { _outputTemplate = SetNewOutputTemplate(value ?? DefaultOutputTemplate); }
        }

        public ITokenFormatStore Formats
        {
            get { return _formats; }
            set { _formats = SetNewFormats(value ?? new TokenFormatStore()); }
        }

        private string _outputTemplate = DefaultOutputTemplate;
        private ITokenFormatStore _formats = new TokenFormatStore();
        private Lazy<IStringLogSerializer> _serializeFunc;
        private ITokenParser _parser = new TokenParser();
        private static PropertyInfo[] _logMsgProps = typeof(LogRecord).GetProperties();

        public StringLogSerializer()
        {
            SetNewFormats(_formats);
        }

        ~StringLogSerializer()
        {
            _formats.Updated -= SetSerializerFunc;
        }

        public string Serialize(LogRecord record)
        {
            return _serializeFunc.Value.Serialize(record);
        }

        object ILogSerializer.Serialize(LogRecord record)
        {
            return Serialize(record);
        }

        private IStringLogSerializer BuildSerializer()
        {
            var tokens = _parser.Parse(OutputTemplate);

            var logExpr = Expression.Parameter(typeof(LogRecord), "record");

            var strExprs = new List<Expression>();

            foreach (var t in tokens)
            {
                var valExpr = default(Expression);

                var msgProp = default(PropertyInfo);

                if (t.IsLiteral)
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
                    else if ((msgProp = _logMsgProps.FirstOrDefault(x => x.Name == t.Value)) != null)
                    {
                        var msgPropExpr = Expression.Property(logExpr, msgProp);

                        valExpr = msgProp.PropertyType == typeof(string)
                            ? (Expression)msgPropExpr
                            : Expression.Call(msgPropExpr, nameof(object.ToString), null);
                    }
                    else
                    {
                        var extraPropExpr = Expression.Property(logExpr, nameof(LogRecord.Extra));

                        var tryGetExtraMI = typeof(CollectionsExtensions)
                                    .GetStaticGenericMethod(
                                        nameof(CollectionsExtensions.TryGet),
                                        genericParamCount: 2,
                                        argsCount: 3
                                    )
                                    .MakeGenericMethod(typeof(string), typeof(string));

                        var callGetExtra = Expression.Call(tryGetExtraMI, extraPropExpr, Expression.Constant(t.Value), Expression.Constant(t.Key));

                        valExpr = callGetExtra;
                    }
                }

                strExprs.Add(valExpr);
            }

            var body = BuildStringConcat(tokens, strExprs);

            var lambda = Expression.Lambda<Func<LogRecord, string>>(body, logExpr);

            var serializerFunc = lambda.CompileFast();

            return new LambdaLogSerializer(serializerFunc);
        }

        private Expression BuildStringConcat(Token[] tokens, List<Expression> strExprs)
        {
            Expression body;

            if (tokens.Length == 1)
            {
                body = strExprs[0];
            }
            else if (tokens.Length == 2)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1]
                    );
            }
            else if (tokens.Length == 3)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1],
                    strExprs[2]
                    );
            }
            else if (tokens.Length == 4)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1],
                    strExprs[2],
                    strExprs[3]
                    );
            }
            else
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string[]) }),
                    Expression.NewArrayInit(typeof(string), strExprs)
                    );
            }

            return body;
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

        private void SetSerializerFunc(object sender = null, EventArgs e = null)
        {
            _serializeFunc = new Lazy<IStringLogSerializer>(BuildSerializer);
        }
    }
}