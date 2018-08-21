using NWrath.Synergy.Common.Structs;

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