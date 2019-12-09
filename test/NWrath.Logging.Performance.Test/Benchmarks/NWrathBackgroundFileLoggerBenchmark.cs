using NWrath.Synergy.Common.Extensions;

namespace NWrath.Logging.Performance.Test
{
    internal class NWrathBackgroundFileLoggerBenchmark
       : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath background file";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;

        protected override void CreateLogger()
        {
            var logger = LoggingWizard.Spell.BackgroundFileLogger(
                tempFile,
                serializerApply: s => s.OutputTemplate = "{Message}"
            );

            logger.BaseLogger
                  .CastTo<FileLogger>()
                  .AutoFlush = false;

            _logger = logger;
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