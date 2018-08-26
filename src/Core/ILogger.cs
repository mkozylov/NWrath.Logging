using System;

namespace NWrath.Logging
{
    public interface ILogger
    {
        ILogLevelVerifier LevelVerifier { get; set; }

        bool IsEnabled { get; set; }

        void Debug(string msg);

        void Info(string msg);

        void Warning(string msg, Exception exception = null);

        void Error(string msg, Exception exception = null);

        void Critical(string msg, Exception exception = null);

        void Log(LogMessage log);

        void Log(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            object extra = null
            );
    }
}