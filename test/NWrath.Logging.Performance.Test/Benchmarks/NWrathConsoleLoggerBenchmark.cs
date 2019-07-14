namespace NWrath.Logging.Performance.Test
{
    internal class NWrathConsoleLoggerBenchmark
       : LoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath console";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.ConsoleLogger(
                serializerApply: s => s.OutputTemplate = "{Message}",
                background: false
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

        protected override void TierDown()
        {
        }

        protected override void SetUp()
        {
        }
    }
}