using NWrath.Synergy.Common.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class PipeLoggerContext<TLogger>
        : IPipeLoggerContext<TLogger>
        where TLogger : ILogger
    {
        public TLogger Logger { get; private set; }

        public LogMessage LogMessage { get; set; }

        public Set Properties { get; } = Set.Empty;

        public PipeLoggerContext(
            TLogger logger,
            LogMessage logMessage,
            Set properties = null
            )
        {
            Logger = logger;
            LogMessage = logMessage;
            Properties = properties ?? new Set();
        }
    }
}