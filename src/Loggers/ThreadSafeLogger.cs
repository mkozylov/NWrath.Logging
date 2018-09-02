using System;

namespace NWrath.Logging
{
    public class ThreadSafeLogger
        : LoggerBase
    {
        public override bool IsEnabled
        {
            get => BaseLogger.IsEnabled;
            set => BaseLogger.IsEnabled = value;
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => BaseLogger.LevelVerifier;
            set => BaseLogger.LevelVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug);
        }

        public ILogger BaseLogger
        {
            get => _baseLogger;
            set { _baseLogger = value ?? throw new ArgumentNullException(Errors.NULL_BASE_LOGGER); }
        }

        private ILogger _baseLogger;
        private object _thisLock = new object();

        public ThreadSafeLogger(ILogger logger)
        {
            BaseLogger = logger;
        }

        public override void Log(LogMessage log)
        {
            WriteLog(log);
        }

        protected override void WriteLog(LogMessage log)
        {
            lock (_thisLock)
            {
                _baseLogger.Log(log);
            }
        }
    }
}