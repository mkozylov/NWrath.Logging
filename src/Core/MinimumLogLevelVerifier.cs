namespace NWrath.Logging
{
    public class MinimumLogLevelVerifier
        : ILogRecordVerifier
    {
        public LogLevel MinimumLevel { get; set; }

        public MinimumLogLevelVerifier(LogLevel minLevel)
        {
            MinimumLevel = minLevel;
        }

        public bool Verify(LogRecord record)
        {
            return record.Level >= MinimumLevel;
        }
    }
}