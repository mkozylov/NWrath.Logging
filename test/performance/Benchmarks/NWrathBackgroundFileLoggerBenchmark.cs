using NWrath.Logging.Performance.Test;
using System;

namespace NWrath.Logging.Performance.Test
{
    internal class NWrathBackgroundFileLoggerBenchmark
       : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath background file logger";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.BackgroundLogger(
                f => f.FileLogger(
                    tempFile,
                    serializerApply: s => s.OutputTemplate = "{Message}",
                    autoFlush: false
                    ));
        }

        protected override void Log(string msg)
        {
            _logger.Info(msg);
        }

        public override void DisposeLogger()
        {
            _logger.Dispose();
        }

        protected override void WarmingUp()
        {
            if (NeedWarmingUp)
            {
                Log("WarmingUp");
            }
        }
    }
}