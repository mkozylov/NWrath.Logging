using NWrath.Synergy.Common.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class RollingFileContext
    {
        public RollingFileLogger Logger { get; private set; }

        public LogMessage LogMessage { get; set; }

        public Set Properties { get; private set; }

        public RollingFileContext(
            RollingFileLogger logger,
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