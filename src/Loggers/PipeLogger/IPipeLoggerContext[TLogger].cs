using NWrath.Synergy.Common.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public interface IPipeLoggerContext<TLogger>
        where TLogger : ILogger
    {
        TLogger Logger { get; }

        LogMessage LogMessage { get; set; }

        Set Properties { get; }
    }
}