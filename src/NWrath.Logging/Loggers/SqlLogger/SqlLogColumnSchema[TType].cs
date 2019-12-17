using System;

namespace NWrath.Logging
{
    public class SqlLogColumnSchema<TType>
        : ISqlLogColumnSchema 
    {
        public string Name { get; }

        public string TypeDefinition { get; }

        public bool IsInternal { get; }

        public Type Type { get; } = typeof(TType);

        public ISqlStringLogSerializer<TType> Serializer { get; set; }

        IStringLogSerializer IDbLogColumnSchema.Serializer { get => Serializer; }

        public SqlLogColumnSchema(
             string name,
             string typeDefinition,
             bool isInternal,
             ISqlStringLogSerializer<TType> serializer = null
             )
        {
            Name = name;
            TypeDefinition = typeDefinition;
            IsInternal = isInternal;
            Serializer = serializer;

            if (!isInternal && serializer == null)
                throw new ArgumentNullException(nameof(serializer));
        }

        public SqlLogColumnSchema(
            string name,
            string typeDefinition,
            bool isInternal,
            Func<LogRecord, TType> serializer
            )
            : this(
            name, 
            typeDefinition, 
            isInternal, 
            new SqlLogSerializer<TType>(serializer)
            )
        {
        }
    }
}