using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class LambdaConsoleLogSerializer
          : IConsoleLogSerializer
    {
        private Func<LogRecord, string> _stringSerializerFunc;

        public LambdaConsoleLogSerializer(Func<LogRecord, string> stringSerializerFunc)
        {
            _stringSerializerFunc = stringSerializerFunc;
        }

        public string Serialize(LogRecord record)
        {
            return _stringSerializerFunc(record);
        }
    }
}
