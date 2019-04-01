using System;
using System.IO;

namespace NWrath.Logging.Performance.Test
{
    internal class NWrathBackgroundRollingFileLoggerBenchmark
        : LoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath background rolling file";

        public bool NeedWarmingUp { get; set; } = true;

        private ILogger _logger;
        private string _folderPath;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.BackgroundLogger(
                f => f.RollingFileLogger(_folderPath)
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

        protected override void SetUp()
        {
            var dir = Path.GetDirectoryName(typeof(FileLoggerBenchmarkBase).Assembly.Location);

            _folderPath = Path.Combine(dir, $"Logs_{Guid.NewGuid()}");
        }

        protected override void TierDown()
        {
            if (!string.IsNullOrEmpty(_folderPath) && Directory.Exists(_folderPath))
            {
                Directory.Delete(_folderPath, true);
            }
        }
    }
}