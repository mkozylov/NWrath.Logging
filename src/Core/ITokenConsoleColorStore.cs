using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public interface ITokenConsoleColorStore
        : IDictionary<string, Func<LogMessage, ConsoleColor>>
    {
        Func<LogMessage, ConsoleColor> Timestamp { get; set; }

        Func<LogMessage, ConsoleColor> Message { get; set; }

        Func<LogMessage, ConsoleColor> Level { get; set; }

        Func<LogMessage, ConsoleColor> Exception { get; set; }

        Func<LogMessage, ConsoleColor> Extra { get; set; }

        ConsoleColor this[string key, LogMessage log] { get; }

        Func<LogMessage, ConsoleColor> this[string key, Func<LogMessage, ConsoleColor> defaultVal = null] { get; set; }

        event EventHandler Updated;
    }
}