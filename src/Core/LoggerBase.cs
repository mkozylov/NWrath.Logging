using NWrath.Synergy.Common.Structs;
using System;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public abstract class LoggerBase
        : ILogger
    {
        public virtual ILogRecordVerifier RecordVerifier
        {
            get => _recordVerifier;
            set { _recordVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug); }
        }

        public virtual bool IsEnabled { get; set; } = true;

        private ILogRecordVerifier _recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Log(LogRecord record)
        {
            if (IsEnabled && VerifyRecord(record))
            {
                WriteRecord(record);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            foreach (var record in batch)
            {
                if (VerifyRecord(record))
                {
                    WriteRecord(record);
                }
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

        public virtual void Log<TExtra>(
           string message,
           DateTime? timestamp = null,
           LogLevel level = LogLevel.Debug,
           Exception exception = null,
           TExtra extra = default(TExtra)
           )
        {
            Log(new LogRecord(message, timestamp, level, exception, (extra as StringSet) ?? StringSet.FromObject(extra)));
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
            return RecordVerifier.Verify(record);
        }

        protected abstract void WriteRecord(LogRecord record);
    }
}