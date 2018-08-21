using System;

namespace NWrath.Logging
{
    public class LogTableColumnSchema
    {
        public string Name { get; }

        public string TypeDefinition { get; }

        public bool IsInternal { get; }

        public ILogSerializer Serializer { get; set; }

        public LogTableColumnSchema(
            string name,
            string typeDefinition,
            bool isInternal,
            ILogSerializer serializer = null
            )
        {
            Name = name;
            TypeDefinition = typeDefinition;
            IsInternal = isInternal;
            Serializer = serializer;
        }

        public LogTableColumnSchema(
            string name,
            string typeDefinition,
            bool isInternal,
            Func<LogMessage, object> serializerLambda
            )
            : this(name, typeDefinition, isInternal, new LambdaLogSerializer(serializerLambda))
        {
        }

        public LogTableColumnSchema(
            string name,
            string typeDefinition,
            bool isInternal,
            string serializerExpr
            )
          : this(name, typeDefinition, isInternal, (LambdaLogSerializer)serializerExpr)
        {
        }
    }
}