using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace NWrath.Logging.Performance.Test
{
    internal class NLogFileLoggerBenchmark
         : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "NLog async file";

        public bool NeedWarmingUp { get; set; } = true;

        private NLog.Logger _logger;

        protected override void CreateLogger()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget
            {
                Name = "FileTarget",
                FileName = tempFile,
                KeepFileOpen = true,
                ConcurrentWrites = false,
                AutoFlush = false,
                Layout = "${message}"
            };

            var asyncFileTarget = new AsyncTargetWrapper(fileTarget)
            {
                TimeToSleepBetweenBatches = 0,
                OverflowAction = AsyncTargetWrapperOverflowAction.Block,
                BatchSize = 100
            };

            config.AddTarget("file", asyncFileTarget);
            config.AddRuleForAllLevels(asyncFileTarget);
            LogManager.Configuration = config;
            LogManager.ReconfigExistingLoggers();
            
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected override void Log(string msg)
        {
            _logger.Info(msg);
        }

        public override void DisposeLogger()
        {
            LogManager.Flush();
            LogManager.Shutdown();
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