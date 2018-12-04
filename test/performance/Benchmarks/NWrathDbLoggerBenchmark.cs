using NWrath.Logging.Performance.Test;
using System;
using System.Threading;

namespace NWrath.Logging.Performance.Test
{
    internal class NWrathDbLoggerBenchmark
        : LoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath db logger";

        public bool NeedWarmingUp { get; set; } = true;

        private BackgroundLogger _logger;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.BackgroundLogger(f => f.DbLogger(
                        "Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True;MultipleActiveResultSets=True",
                        s => s.Columns = new[] { LogTableSchema.IdColumn, LogTableSchema.MessageColumn }
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

        protected override void SetUp()
        {
        }

        protected override void TierDown()
        {
        }
    }
}