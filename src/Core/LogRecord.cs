using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging
{
    public class LogRecord
    {
        public static LogRecord Empty { get; } = new LogRecord { Timestamp = DateTime.MinValue };

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public LogLevel Level { get; set; }

        public StringSet Extra { get; set; }

        public LogRecord()
        {
            Timestamp = DateTime.Now;
            Extra = new StringSet();
        }

        public LogRecord(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            StringSet extra = null
            )
        {
            Message = message;
            Timestamp = timestamp ?? DateTime.Now;
            Level = level;
            Exception = exception;
            Extra = extra ?? new StringSet();
        }

        public LogRecord Clone()
        {
            return (LogRecord)MemberwiseClone();
        }
    }
}