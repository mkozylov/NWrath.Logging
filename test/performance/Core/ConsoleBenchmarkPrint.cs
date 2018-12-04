using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging.Performance.Test
{
    public class ConsoleBenchmarkPrint
    {
        public void Print(BenchmarkCaseGroup group, BenchmarkCaseGroupResult result)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(group.Info);
            Console.ResetColor();
            Console.WriteLine();

            foreach (var cs in result.Cases)
            {
                Console.WriteLine("Items {0}", cs.Key);

                var table = new ConsoleTables.ConsoleTable(
                    "Logger",
                    "Total(ms)",
                    "Create(ms)",
                    "Write(ms)",
                    "Dispose(ms)"
                    );

                table.Options.EnableCount = false;

                foreach (var bc in cs.Value)
                {
                    table.AddRow(
                        $"{bc.LoggerInfo}",
                        $"{bc.BenchmarkResult.TotalTimeMs}",
                        $"{bc.BenchmarkResult.CreateTimeMs}",
                        $"{bc.BenchmarkResult.WriteTimeMs}",
                        $"{bc.BenchmarkResult.DisposeTimeMs}"
                        );
                }

                table.Write();
            }

            var resultTable = new ConsoleTables.ConsoleTable("Logger", "Rank");

            resultTable.Options.EnableCount = false;

            foreach (var place in result.Ranks)
            {
                resultTable.AddRow($"{place.Key}", $"{place.Value}");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Results:");
            Console.ResetColor();

            resultTable.Write(Format.Alternative);

            Console.WriteLine(new string('#', 50));
            Console.WriteLine();
        }
    }
}