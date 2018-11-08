using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public class RollingFileContext
    {
        public RollingFileLogger Logger { get; private set; }

        public LogRecord LogRecord { get; set; }

        public Set Properties { get; private set; }

        public RollingFileContext(
            RollingFileLogger logger,
            LogRecord logRecord,
            Set properties = null
            )
        {
            Logger = logger;
            LogRecord = logRecord;
            Properties = properties ?? new Set();
        }
    }
}