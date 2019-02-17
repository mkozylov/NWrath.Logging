using System;
using System.IO;

namespace NWrath.Logging.Performance.Test
{
    internal abstract class FileLoggerBenchmarkBase : LoggerBenchmarkBase
    {
        protected string tempFile;

        protected override void SetUp()
        {
            var dir = Path.GetDirectoryName(typeof(FileLoggerBenchmarkBase).Assembly.Location);

            tempFile = Path.Combine(dir, $"{Guid.NewGuid()}.log");
        }

        protected override void TierDown()
        {
            if (!string.IsNullOrEmpty(tempFile) && File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}