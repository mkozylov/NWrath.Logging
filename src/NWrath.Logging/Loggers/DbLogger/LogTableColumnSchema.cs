using System;

namespace NWrath.Logging
{
    public class LogTableColumnSchema
    {
        public string Name { get; }

        public string TypeDefinition { get; }

        public bool IsInternal { get; }

        public ILogSerializer Serializer { get; set; }

        public Type Type { get; set; }

        public LogTableColumnSchema(
             string name,
             string typeDefinition,
             bool isInternal,
             Type type,
             ILogSerializer serializer = null
             )
        {
            Name = name;
            TypeDefinition = typeDefinition;
            IsInternal = isInternal;
            Type = type;
            Serializer = serializer;

            if (!isInternal && serializer == null) throw new ArgumentNullException(nameof(serializer));
        }

        public LogTableColumnSchema(
            string name,
            string typeDefinition,
            bool isInternal,
            Type type,
            Func<LogRecord, object> serializerLambda
            )
            : this(name, typeDefinition, isInternal, type, new LambdaLogSerializer(serializerLambda))
        {
        }

        public LogTableColumnSchema UseSerializer(Func<LogRecord, object> serializerLambda)
        {
            Serializer = new LambdaLogSerializer(serializerLambda);

            return this;
        }
    }
}