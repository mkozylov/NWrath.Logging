using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NWrath.Logging.Performance.Test
{
    public abstract class LoggerBenchmarkBase
    {
        public abstract string LoggerInfo { get; set; }

        public int ItemsCount = 1;

        private Stopwatch _logSw;
        private Stopwatch _disposeSw;
        private Stopwatch _createSw;

        public virtual BenchmarkResult DoMeasure()
        {
            SetUp();

            _createSw = Stopwatch.StartNew();
            CreateLogger();
            _createSw.Stop();

            WarmingUp();

            _logSw = Stopwatch.StartNew();

            for (var i = 0; i < ItemsCount; i++)
            {
                FormatAndLog(i, ItemsCount);
            }

            _logSw.Stop();

            _disposeSw = Stopwatch.StartNew();
            DisposeLogger();
            _disposeSw.Stop();

            var result = GetResults();

            TierDown();

            return result;
        }

        public abstract void DisposeLogger();

        protected abstract void SetUp();

        protected abstract void TierDown();

        protected abstract void CreateLogger();

        protected abstract void Log(string msg);

        protected abstract void WarmingUp();

        protected virtual void FormatAndLog(int msgId, int totalCount)
        {
            Log($"Log message {msgId + 1} / {totalCount}");
        }

        protected virtual BenchmarkResult GetResults()
        {
            return new BenchmarkResult
            {
                TotalTimeMs = _createSw.Elapsed.TotalMilliseconds + _disposeSw.Elapsed.TotalMilliseconds + _logSw.Elapsed.TotalMilliseconds,
                CreateTimeMs = _createSw.Elapsed.TotalMilliseconds,
                WriteTimeMs = _logSw.Elapsed.TotalMilliseconds,
                DisposeTimeMs = _disposeSw.Elapsed.TotalMilliseconds
            };
        }
    }
}