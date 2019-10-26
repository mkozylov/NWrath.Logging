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
            _logger = LoggingWizard.Spell.BackgroundFileLogger(
                tempFile,
                serializerApply: s => s.OutputTemplate = "{Message}"
            );
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