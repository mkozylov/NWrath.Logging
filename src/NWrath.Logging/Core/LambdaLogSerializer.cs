using System;

namespace NWrath.Logging
{
    public class LambdaLogSerializer
        : IStringLogSerializer, ILogSerializer
    {
        private Func<LogRecord, string> _stringSerializerFunc;

        private Func<LogRecord, object> _serializerFunc;

        public LambdaLogSerializer(Func<LogRecord, string> stringSerializerFunc)
        {
            _stringSerializerFunc = stringSerializerFunc;

            _serializerFunc = stringSerializerFunc;
        }

        public LambdaLogSerializer(Func<LogRecord, object> serializerFunc)
        {
            _serializerFunc = serializerFunc;

            _stringSerializerFunc = m => serializerFunc(m)?.ToString();
        }

        public string Serialize(LogRecord record)
        {
            return _stringSerializerFunc(record);
        }

        object ILogSerializer.Serialize(LogRecord record)
        {
            return _serializerFunc(record);
        }
    }
}