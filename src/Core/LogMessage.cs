using NWrath.Synergy.Common.Structs;
using System;
using System.Collections.Generic;
using System.Text;

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
            StringSet extra = null
            )
        {
            Message = message;
            Timestamp = timestamp ?? DateTime.Now;
            Level = level;
            Extra = extra ?? StringSet.Empty;
        }

        public LogMessage Clone()
        {
            return (LogMessage)MemberwiseClone();
        }
    }
}