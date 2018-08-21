using NWrath.Synergy.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NWrath.Logging
{
    public class RollingFileProvider
        : IRollingFileProvider
    {
        public string FolderPath { get; set; }

        public Regex FileMatcher { get; set; }

        public RollingFileProvider(
            string folderPath
            )
        {
            FolderPath = folderPath;
            FileMatcher = new Regex(@"\d{8}-\d{5}\.log", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(FolderPath)
                            .Where(x => FileMatcher.IsMatch(x))
                            .ToArray();
        }

        public string ProduceNewFile()
        {
            var today = Clock.Today;

            var todayFilesCount = GetFiles().Select(x => new FileInfo(x))
                                            .Count(x => x.CreationTime.Date == today);

            return Path.Combine(FolderPath, $"{today:yyyyMMdd}-{(todayFilesCount + 1):D5}.log");
        }

        public string TryResolveLastFile()
        {
            var lastFile = GetFiles().Select(x => new FileInfo(x))
                                     .OrderByDescending(x => x.CreationTime)
                                     .FirstOrDefault();

            return lastFile?.FullName;
        }
    }
}