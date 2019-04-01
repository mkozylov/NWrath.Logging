using NWrath.Synergy.Common.Structs;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public abstract class LoggerBase
        : ILogger
    {
        public virtual ILogRecordVerifier RecordVerifier
        {
            get => _recordVerifier;
            set => _recordVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug);
        }

        public virtual bool IsEnabled { get; set; } = true;

        private ILogRecordVerifier _recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Log(LogRecord record)
        {
            if (IsEnabled && RecordVerifier.Verify(record))
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
                if (RecordVerifier.Verify(record))
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

        public virtual void Debug(string msg, StringSet extra = null)
        {
            Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public virtual void Debug<TExtra>(string msg, TExtra extra = default(TExtra))
        {
            Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public virtual void Info(string msg, StringSet extra = null)
        {
            Log(msg, level: LogLevel.Info, extra: extra);
        }

        public virtual void Info<TExtra>(string msg, TExtra extra = default(TExtra))
        {
            Log(msg, level: LogLevel.Info, extra: extra);
        }

        public virtual void Warning(string msg, Exception exception = null, StringSet extra = null)
        {
            Log(msg, level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public virtual void Warning<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra))
        {
            Log(msg, level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public void Warning(
            Exception exception, 
            [CallerMemberName] string callerMemberName = null, 
            [CallerFilePath] string callerFilePath = null, 
            StringSet extra = null
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public void Warning<TExtra>(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public virtual void Error(string msg, Exception exception = null, StringSet extra = null)
        {
            Log(msg, level: LogLevel.Error, exception: exception, extra: extra);
        }

        public virtual void Error<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra))
        {
            Log(msg, level: LogLevel.Error, exception: exception, extra: extra);
        }

        public void Error(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null, 
            StringSet extra = null
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Error, exception: exception, extra: extra);
        }

        public void Error<TExtra>(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Error, exception: exception, extra: extra);
        }

        public virtual void Critical(string msg, Exception exception = null, StringSet extra = null)
        {
            Log(msg, level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public virtual void Critical<TExtra>(string msg, Exception exception = null, TExtra extra = default(TExtra))
        {
            Log(msg, level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public void Critical(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public void Critical<TExtra>(
            Exception exception, 
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
            )
        {
            Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public virtual void Dispose()
        {
        }

        protected abstract void WriteRecord(LogRecord record);

        #region private

        private static string ExtractCaller(string callerMemberName, string callerFilePath)
        {
            return $"{Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMemberName}";
        } 

        #endregion
    }
}