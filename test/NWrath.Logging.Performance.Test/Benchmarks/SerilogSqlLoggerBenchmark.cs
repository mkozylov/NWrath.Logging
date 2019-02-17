using NWrath.Logging.Performance.Test;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Linq;

namespace NWrath.Logging.Performance.Test
{
    internal class SerilogSqlLoggerBenchmark
        : FileLoggerBenchmarkBase
    {
        public override string LoggerInfo { get; set; } = "Serilog sql";

        public bool NeedWarmingUp { get; set; } = true;

        private Serilog.ILogger _logger;

        protected override void CreateLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    "Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True;MultipleActiveResultSets=True",
                    "SerilogSqlLog",
                    autoCreateSqlTable: true,
                    period: TimeSpan.FromSeconds(1),
                    columnOptions: new ColumnOptions { Store = new[] { StandardColumn.Message }.ToList() }
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