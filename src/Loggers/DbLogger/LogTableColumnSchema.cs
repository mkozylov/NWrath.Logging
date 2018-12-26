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
            Func<LogRecord, object> serializerLambda
            )
            : this(name, typeDefinition, isInternal, new LambdaLogSerializer(serializerLambda))
        {
        }

        public LogTableColumnSchema UseSerializer(Func<LogRecord, object> serializerLambda)
        {
            Serializer = new LambdaLogSerializer(serializerLambda);

            return this;
        }
    }
}