using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging
{
    public interface ILogger
        : IDisposable
    {
        ILogLevelVerifier LevelVerifier { get; set; }

        bool IsEnabled { get; set; }

        void Debug(string msg);

        void Info(string msg);

        void Warning(string msg, Exception exception = null);

        void Error(string msg, Exception exception = null);

        void Critical(string msg, Exception exception = null);

        void Log(LogRecord record);

        void Log(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            StringSet extra = null
            );

        void Log(LogRecord[] batch);
    }
}