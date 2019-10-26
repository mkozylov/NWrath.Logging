using System;

namespace NWrath.Logging
{
    public interface IDbLogColumnSchema
    {
        string Name { get; }

        string TypeDefinition { get; }

        bool IsInternal { get; }

        IStringLogSerializer Serializer { get; }

        Type Type { get; }
    }
}