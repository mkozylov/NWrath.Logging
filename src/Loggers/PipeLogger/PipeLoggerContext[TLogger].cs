using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public class PipeLoggerContext<TLogger>
        : IPipeLoggerContext<TLogger>
        where TLogger : ILogger
    {
        public TLogger Logger { get; private set; }

        public LogRecord LogRecord { get; set; }

        public Set Properties { get; } = Set.Empty;

        public PipeLoggerContext(
            TLogger logger,
            LogRecord logRecord,
            Set properties = null
            )
        {
            Logger = logger;
            LogRecord = logRecord;
            Properties = properties ?? Properties;
        }
    }
}