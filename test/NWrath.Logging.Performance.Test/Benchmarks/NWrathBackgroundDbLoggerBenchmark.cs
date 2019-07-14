namespace NWrath.Logging.Performance.Test
{
    internal class NWrathBackgroundDbLoggerBenchmark
        : LoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath background db";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.DbLogger(s =>
                      {
                          s.ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True;MultipleActiveResultSets=True";
                          s.TableName = "BackgroundDbLog";
                          s.Columns = new[] { SqlLogSchema.IdColumn, SqlLogSchema.MessageColumn };
                      }, background: true);
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

        protected override void SetUp()
        {
        }

        protected override void TierDown()
        {
        }
    }
}