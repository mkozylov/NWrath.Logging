using System;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using NWrath.Synergy.Common.Extensions;
using System.IO;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Common.Structs;
using System.Collections.Generic;

namespace NWrath.Logging.Performance.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var items = new[] { 64000 };

            var benchmarks = new LoggerBenchmarkBase[]
            {
                new NWrathFileLoggerBenchmark(),
                new NWrathBackgroundFileLoggerBenchmark(),
                new SerilogFileLoggerBenchmark(),
                new NLogFileLoggerBenchmark(),

                //new SerilogSqlLoggerBenchmark(),
                //new NWrathDbLoggerBenchmark(),
                //new NWrathBackgroundDbLoggerBenchmark()

                new NWrathRollingFileLoggerBenchmark(),
                new NWrathBackgroundRollingFileLoggerBenchmark()
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