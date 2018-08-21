namespace NWrath.Logging
{
    public class ThreadSafeLogger
        : LoggerBase
    {
        public override bool IsEnabled
        {
            get => SafeLogger.IsEnabled;
            set => SafeLogger.IsEnabled = value;
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => SafeLogger.LevelVerifier;
            set => SafeLogger.LevelVerifier = value;
        }

        public ILogger SafeLogger { get; set; }

        private object _thisLock = new object();

        public ThreadSafeLogger(ILogger logger)
        {
            SafeLogger = logger;
        }

        public override void Log(LogMessage log)
        {
            WriteLog(log);
        }

        protected override void WriteLog(LogMessage log)
        {
            lock (_thisLock)
            {
                SafeLogger.Log(log);
            }
        }
    }
}