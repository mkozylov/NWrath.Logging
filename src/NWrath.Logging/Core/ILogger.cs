using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging
{
    public interface ILogger
        : IDisposable
    {
        ILogRecordVerifier RecordVerifier { get; set; }

        bool IsEnabled { get; set; }

        void Debug(string msg, StringSet extra = null);

        void Debug<TExtra>(string msg, TExtra extra = default(TExtra));

        void Info(string msg, StringSet extra = null);

        void Info<TExtra>(string msg, TExtra extra = default(TExtra));

        void Warning(string msg, Exception exception = null, StringSet extra = null);

        void Warning<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra));

        void Error(string msg, Exception exception = null, StringSet extra = null);

        void Error<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra));

        void Critical(string msg, Exception exception = null, StringSet extra = null);

        void Critical<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra));

        void Log(LogRecord record);

        void Log(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            StringSet extra = null
            );

        void Log<TExtra>(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            TExtra extra = default(TExtra)
            );

        void Log(LogRecord[] batch);
    }
}