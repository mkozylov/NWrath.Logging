using NWrath.Synergy.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NWrath.Logging
{
    public class RollingFileProvider
        : IRollingFileProvider
    {
        public DirectoryInformation Directory { get; set; }

        public Regex FileMatcher { get; set; }

        public RollingFileProvider(
            string folderPath
            )
        {
            Directory = new DirectoryInformation(folderPath);
            FileMatcher = new Regex(@"\d{8}-\d{5}\.log", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public FileInformation[] GetFiles()
        {
            return Directory.EnumerateFiles()
                            .Where(x => FileMatcher.IsMatch(x.FullName))
                            .ToArray();
        }

        public FileInformation ProduceNewFile()
        {
            var today = Clock.Today;

            var todayFilesCount = GetFiles().Count(x => x.CreationTime.Date == today);

            return new FileInformation(
                Path.Combine(Directory.FullName, $"{today:yyyyMMdd}-{(todayFilesCount + 1):D5}.log")
                );
        }

        public FileInformation TryResolveLastFile()
        {
            var lastFile = GetFiles().OrderByDescending(x => x.CreationTime)
                                     .FirstOrDefault();

            return lastFile;
        }
    }
}