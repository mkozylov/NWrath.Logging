﻿namespace NWrath.Logging.Performance.Test
{
    internal class NWrathDbLoggerBenchmark
        : LoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath sql";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.SqlLogger(s =>
                      {
                          s.ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True;MultipleActiveResultSets=True";
                          s.TableName = "SqlLog";
                          s.Columns = new[] { SqlLogSchema.IdColumn, SqlLogSchema.MessageColumn };
                      });
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