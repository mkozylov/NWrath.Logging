using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public interface ITokenFormatStore
        : IDictionary<string, Func<LogRecord, string>>
    {
        Func<LogRecord, string> Exception { get; set; }

        Func<LogRecord, string> Level { get; set; }

        Func<LogRecord, string> Message { get; set; }

        Func<LogRecord, string> Timestamp { get; set; }

        string this[string key, LogRecord record] { get; }

        Func<LogRecord, string> this[string key, Func<LogRecord, string> defaultVal = null] { get; set; }

        event EventHandler Updated;
    }
}