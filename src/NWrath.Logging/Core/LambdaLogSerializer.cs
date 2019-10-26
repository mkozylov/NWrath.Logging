using System;

namespace NWrath.Logging
{
    public class LambdaLogSerializer
        : IStringLogSerializer
    {
        private Func<LogRecord, string> _stringSerializerFunc;

        public LambdaLogSerializer(Func<LogRecord, string> stringSerializerFunc)
        {
            _stringSerializerFunc = stringSerializerFunc;
        }

        public string Serialize(LogRecord record)
        {
            return _stringSerializerFunc(record);
        }
    }
}