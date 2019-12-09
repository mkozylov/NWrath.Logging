using FastExpressionCompiler;
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
    public class StringLogSerializerBuilder
        : StringSerializerBuilderBase<LogRecord, StringLogSerializerBuilder, RecordFormatStore, IStringLogSerializer>
    {
        public const string DefaultOutputTemplate = "{Timestamp} [{Level}] {Message}{exnl}{Exception}";
        public static IStringLogSerializer DefaultSerializer { get; } = BuildDefaultSerializer();

        public override string OutputTemplate { get; set; } = DefaultOutputTemplate;

        public override IStringLogSerializer BuildSerializer()
        {
            var serializerFunc = BuildLambda();

            return new LambdaLogSerializer(serializerFunc);
        }

        private static IStringLogSerializer BuildDefaultSerializer()
        {
            return new StringLogSerializerBuilder()
                .UseOutputTemplate(DefaultOutputTemplate)
                .BuildSerializer();
        }
    }
}