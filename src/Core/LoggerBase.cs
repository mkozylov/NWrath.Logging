using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging
{
    public abstract class LoggerBase
        : ILogger
    {
        public virtual ILogLevelVerifier LevelVerifier { get => _levelVerifier; set { _levelVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug); } }

        public virtual bool IsEnabled { get; set; } = true;

        private ILogLevelVerifier _levelVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

        public virtual void Log(LogRecord record)
        {
            if (IsEnabled && VerifyRecord(record))
            {
                WriteRecord(record);
            }
        }

        public virtual void Log(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            StringSet extra = null
            )
        {
            Log(new LogRecord(message, timestamp, level, exception, extra));
        }

        public virtual void Log(LogRecord[] batch)
        {
            if (!IsEnabled)
            {
                return;
            }

            foreach (var log in batch)
            {
                if (VerifyRecord(log))
                {
                    WriteRecord(log);
                }
            }
        }

        public virtual void Debug(string msg)
        {
            var logMsg = new LogRecord
            {
                Message = msg,
                Level = LogLevel.Debug
            };

            Log(logMsg);
        }

        public virtual void Info(string msg)
        {
            var logMsg = new LogRecord
            {
                Message = msg,
                Level = LogLevel.Info
            };

            Log(logMsg);
        }

        public virtual void Warning(string msg, Exception exception = null)
        {
            var logMsg = new LogRecord
            {
                Message = msg,
                Level = LogLevel.Warning,
                Exception = exception
            };

            Log(logMsg);
        }

        public virtual void Error(string msg, Exception exception = null)
        {
            var logMsg = new LogRecord
            {
                Message = msg,
                Level = LogLevel.Error,
                Exception = exception
            };

            Log(logMsg);
        }

        public virtual void Critical(string msg, Exception exception = null)
        {
            var logMsg = new LogRecord
            {
                Message = msg,
                Level = LogLevel.Critical,
                Exception = exception
            };

            Log(logMsg);
        }

        public virtual void Dispose()
        {
        }

        protected virtual bool VerifyRecord(LogRecord record)
        {
            return LevelVerifier.Verify(record.Level);
        }

        protected abstract void WriteRecord(LogRecord record);
    }
}