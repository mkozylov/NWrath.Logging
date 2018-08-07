using System;
using System.Linq;
using System.Linq.Expressions;

namespace NWrath.Logging
{
    public abstract class LoggerBase
        : ILogger
    {
        public virtual ILogLevelVerifier LevelVerifier { get; set; } = new MinimumLogLevelVerifier(LogLevel.Debug);

        public virtual bool IsEnabled { get; set; } = true;

        public virtual void Log(LogMessage log)
        {
            if (IsEnabled && LevelVerifier.Verify(log.Level))
            {
                WriteLog(log);
            }
        }

        public virtual void Debug(string msg)
        {
            var logMsg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = msg,
                Level = LogLevel.Debug
            };

            Log(logMsg);
        }

        public virtual void Info(string msg)
        {
            var logMsg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = msg,
                Level = LogLevel.Info
            };

            Log(logMsg);
        }

        public virtual void Warning(string msg, Exception exception = null)
        {
            var logMsg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = msg,
                Level = LogLevel.Warning,
                Exception = exception
            };

            Log(logMsg);
        }

        public virtual void Error(string msg, Exception exception = null)
        {
            var logMsg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = msg,
                Level = LogLevel.Error,
                Exception = exception
            };

            Log(logMsg);
        }

        public virtual void Critical(string msg, Exception exception = null)
        {
            var logMsg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = msg,
                Level = LogLevel.Critical,
                Exception = exception
            };

            Log(logMsg);
        }

        protected abstract void WriteLog(LogMessage log);
    }
}