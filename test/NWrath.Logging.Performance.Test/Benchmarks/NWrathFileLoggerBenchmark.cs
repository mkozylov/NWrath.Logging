using System.Threading;

namespace NWrath.Logging.Performance.Test
{
    internal class NWrathFileLoggerBenchmark
        : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NWrath file";

        public bool NeedWarmingUp { get; set; } = true;

        private FileLogger _logger;
        private Timer _flushTask;

        protected override void CreateLogger()
        {
            _logger = LoggingWizard.Spell.FileLogger(
                    tempFile,
                    serializerApply: s => s.OutputTemplate = "{Message}",
                    autoFlush: false
                    );

            _flushTask = new Timer(_ => _logger.Flush(), null, 1000, 1000);
        }

        protected override void Log(string msg)
        {
            _logger.Info(msg);
        }

        public override void DisposeLogger()
        {
            _flushTask.Dispose();

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