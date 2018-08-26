using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging
{
    public class LogMessage
    {
        public static LogMessage Empty { get; } = new LogMessage(null, timestamp: DateTime.MinValue);

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public LogLevel Level { get; set; } = LogLevel.Debug;

        public StringSet Extra { get; set; } = StringSet.Empty;

        public LogMessage()
        {
        }

        public LogMessage(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            object extra = null
            )
        {
            Message = message;
            Timestamp = timestamp ?? DateTime.Now;
            Level = level;
            Exception = exception;

            if (extra != null)
            {
                Extra = (extra as StringSet) ?? StringSet.FromObject(extra);
            }
        }

        public LogMessage Clone()
        {
            return (LogMessage)MemberwiseClone();
        }
    }
}