using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging.Performance.Test
{
    public class BenchmarkCaseGroupResult
    {
        public Dictionary<int, BenchmarkCase[]> Cases { get; set; }

        public Dictionary<string, int> Ranks { get; set; }
    }
}