using NWrath.Logging.Performance.Test;
using Serilog;
using System;

namespace NWrath.Logging.Performance.Test
{
    internal class SerilogFileLoggerBenchmark
        : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "Serilog file";

        public bool NeedWarmingUp { get; set; } = true;

        private Serilog.ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File(
                    tempFile,
                    buffered: true,
                    outputTemplate: "{Message}",
                    flushToDiskInterval: TimeSpan.FromMilliseconds(1000)
                )
                .CreateLogger();

            Serilog.Log.Logger = _logger;
        }

        protected override void Log(string msg)
        {
            _logger.Information(msg);
        }

        public override void DisposeLogger()
        {
            Serilog.Log.CloseAndFlush();
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