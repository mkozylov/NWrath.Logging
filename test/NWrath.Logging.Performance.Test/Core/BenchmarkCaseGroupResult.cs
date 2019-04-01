using System.Collections.Generic;

namespace NWrath.Logging.Performance.Test
{
    public class BenchmarkCaseGroupResult
    {
        public Dictionary<int, BenchmarkCase[]> Cases { get; set; }

        public Dictionary<string, int> Ranks { get; set; }
    }
}