namespace NWrath.Logging
{
    public class MinimumLogLevelVerifier
        : ILogLevelVerifier
    {
        public LogLevel MinimumLevel { get; private set; } = LogLevel.Debug;

        public MinimumLogLevelVerifier(LogLevel minLevel)
        {
            SetMinimumLevel(minLevel);
        }

        public void SetMinimumLevel(LogLevel minLevel)
        {
            MinimumLevel = minLevel;
        }

        public bool Verify(LogLevel level)
        {
            return level >= MinimumLevel;
        }
    }
}