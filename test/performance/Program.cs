using NWrath.Logging.Performance.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;
using System.Data;
using System.Threading;
using System.Globalization;
using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging.Performance.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = LoggingWizard.Spell.DbLogger("Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True");

            logger.Log("qwe");

            logger.Dispose();

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var items = new[] { 64000 };

            var benchmarks = new LoggerBenchmarkBase[]
            {
                new NWrathFileLoggerBenchmark(),
                new NWrathBackgroundFileLoggerBenchmark(),
                new SerilogFileLoggerBenchmark(),
                new NLogFileLoggerBenchmark()
            };

            new BenchmarkCaseGroup
            {
                Info = "run once for code precompile",
                ItemsCounts = new[] { 1 },
                Benchmarks = benchmarks
            }.Start();

            var caseGroup = new BenchmarkCaseGroup
            {
                Info = "File loggers benchmarks",
                ItemsCounts = items,
                Benchmarks = benchmarks
            };

            var result = caseGroup.Start();

            new ConsoleBenchmarkPrint()
                .Print(caseGroup, result);

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}