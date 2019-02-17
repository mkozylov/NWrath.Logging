using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging.Performance.Test
{
    public class BenchmarkCaseGroup
    {
        public string Info { get; set; }

        public int[] ItemsCounts { get; set; }

        public LoggerBenchmarkBase[] Benchmarks { get; set; }

        public BenchmarkCaseGroupResult Start()
        {
            var cases = new Dictionary<int, BenchmarkCase[]>();

            foreach (var itemsCount in ItemsCounts)
            {
                var temp = new List<BenchmarkCase>();

                foreach (var b in Benchmarks)
                {
                    b.ItemsCount = itemsCount;

                    var perf = new BenchmarkCase
                    {
                        LoggerInfo = b.LoggerInfo,
                        ItemsCount = itemsCount,
                        BenchmarkResult = b.DoMeasure()
                    };

                    temp.Add(perf);
                }

                cases.Add(
                    itemsCount,
                    temp.OrderBy(x => x.BenchmarkResult.TotalTimeMs).ToArray()
                    );
            }

            foreach (var c in cases)
            {
                for (int i = 0; i < c.Value.Length; i++)
                {
                    var perfCase = c.Value[i];

                    perfCase.LoggerPoints = i + 1;
                }
            }

            var loggerPoints = new Dictionary<string, int>();
            var perfMeters = cases.SelectMany(x => x.Value).ToList();

            foreach (var b in Benchmarks)
            {
                var points = perfMeters.Where(x => x.LoggerInfo == b.LoggerInfo)
                                       .Sum(x => x.LoggerPoints);

                loggerPoints.Add(b.LoggerInfo, points);
            }

            var loggerPlaces = loggerPoints.OrderBy(x => x.Value)
                                           .GroupBy(x => x.Value)
                                           .ToDictionary(k => k.Key, v => v.ToArray());

            var result = new BenchmarkCaseGroupResult
            {
                Cases = cases,
                Ranks = new Dictionary<string, int>()
            };

            for (int i = 0; i < loggerPlaces.Count; i++)
            {
                foreach (var l in loggerPlaces.ElementAt(i).Value)
                {
                    result.Ranks.Add(l.Key, i + 1);
                }
            }

            return result;
        }
    }
}