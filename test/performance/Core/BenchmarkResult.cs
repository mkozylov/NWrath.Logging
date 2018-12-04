namespace NWrath.Logging.Performance.Test
{
    public class BenchmarkResult
    {
        public double TotalTimeMs { get; set; }

        public double CreateTimeMs { get; set; }

        public double WriteTimeMs { get; set; }

        public double DisposeTimeMs { get; set; }
    }
}