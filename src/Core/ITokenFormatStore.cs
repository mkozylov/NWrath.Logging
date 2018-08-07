using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public interface ITokenFormatStore
        : IDictionary<string, Func<LogMessage, string>>
    {
        Func<LogMessage, string> Exception { get; set; }

        Func<LogMessage, string> Level { get; set; }

        Func<LogMessage, string> Message { get; set; }

        Func<LogMessage, string> Timestamp { get; set; }

        string this[string key, LogMessage log] { get; }

        Func<LogMessage, string> this[string key, Func<LogMessage, string> defaultVal = null] { get; set; }

        event EventHandler Updated;
    }
}