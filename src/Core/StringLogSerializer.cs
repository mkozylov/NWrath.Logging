using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Reflection.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public class StringLogSerializer
        : IStringLogSerializer, ILogSerializer
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

        private string _outputTemplate = DefaultOutputTemplate;
        private ITokenFormatStore _formats = new TokenFormatStore();
        private Lazy<IStringLogSerializer> _serializeFunc;
        private ITokenParser _parser = new TokenParser();
        private static PropertyInfo[] _logMsgProps = typeof(LogMessage).GetProperties();

        public StringLogSerializer()
        {
            SetNewFormats(_formats);
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
            var tokens = _parser.Parse(OutputTemplate);

            var logExpr = Expression.Parameter(typeof(LogMessage), "log");

            var strExprs = new List<Expression>();

            foreach (var t in tokens)
            {
                var valExpr = default(Expression);

                var msgProp = default(PropertyInfo);

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
                    else if ((msgProp = _logMsgProps.FirstOrDefault(x => x.Name == t.Value)) != null)
                    {
                        var msgPropExpr = Expression.Property(logExpr, msgProp);

                        valExpr = Expression.Call(msgPropExpr, nameof(object.ToString), null);
                    }
                    else
                    {
                        var extraPropExpr = Expression.Property(logExpr, nameof(LogMessage.Extra));

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

            var body = Expression.Call(
                typeof(string).GetMethod(nameof(string.Concat),
                new[] { typeof(string[]) }),
                Expression.NewArrayInit(typeof(string), strExprs)
                );

            var lambda = Expression.Lambda<Func<LogMessage, string>>(body, logExpr);

            var serializerFunc = lambda.Compile();

            return new LambdaLogSerializer(serializerFunc);
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