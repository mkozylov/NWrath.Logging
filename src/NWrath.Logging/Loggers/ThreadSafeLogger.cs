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
        private bool _leaveOpen;

        public ThreadSafeLogger(
            ILogger logger,
            bool leaveOpen = false
            )
        {
            BaseLogger = logger;
            _leaveOpen = leaveOpen;
        }

        ~ThreadSafeLogger()
        {
            Dispose();
        }

        public override void Log(LogRecord record)
        {
            lock (_thisLock)
            {
                _baseLogger.Log(record);
            }
        }

        public override void Log(LogRecord[] batch)
        {
            lock (_thisLock)
            {
                _baseLogger.Log(batch);
            }
        }

        public override void Dispose()
        {
            if (!_leaveOpen)
            {
                _baseLogger.Dispose();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            _baseLogger.Log(record);
        }
    }
}