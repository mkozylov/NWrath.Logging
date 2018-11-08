using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public interface ITokenConsoleColorStore
        : IDictionary<string, Func<LogRecord, ConsoleColor>>
    {
        Func<LogRecord, ConsoleColor> Timestamp { get; set; }

        Func<LogRecord, ConsoleColor> Message { get; set; }

        Func<LogRecord, ConsoleColor> Level { get; set; }

        Func<LogRecord, ConsoleColor> Exception { get; set; }

        Func<LogRecord, ConsoleColor> Extra { get; set; }

        ConsoleColor this[string key, LogRecord record] { get; }

        Func<LogRecord, ConsoleColor> this[string key, Func<LogRecord, ConsoleColor> defaultVal = null] { get; set; }

        event EventHandler Updated;
    }
}