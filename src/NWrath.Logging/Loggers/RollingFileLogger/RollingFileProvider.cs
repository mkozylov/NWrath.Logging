using NWrath.Synergy.Common;
using System;
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

        private Func<FileInformation> _newFileFactory;

        public RollingFileProvider(
            string folderPath,
            bool useSimpleFormat = true
            )
        {
            Directory = new DirectoryInformation(folderPath);

            var pattern = string.Empty;

            if (useSimpleFormat)
            {
                pattern = @"\d{8}\.log$";
                _newFileFactory = ProduceNewFileSimple;
            }
            else
            {
                pattern = @"\d{8}-\d{5}\.log$";
                _newFileFactory = ProduceNewFileAdvance;
            }

            FileMatcher = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public virtual FileInformation[] GetFiles()
        {
            return Directory.EnumerateFiles()
                            .Where(x => FileMatcher.IsMatch(x.FullName))
                            .ToArray();
        }

        public virtual FileInformation ProduceNewFile()
        {
            return _newFileFactory();
        }

        public virtual FileInformation TryResolveLastFile()
        {
            var lastFile = GetFiles().OrderByDescending(x => x.CreationTime)
                                     .FirstOrDefault();

            return lastFile;
        }

        private FileInformation ProduceNewFileSimple()
        {
            return new FileInformation(
                Path.Combine(Directory.FullName, $"{Clock.Today:yyyyMMdd}.log")
                );
        }

        private FileInformation ProduceNewFileAdvance()
        {
            var today = Clock.Today;

            var todayFilesCount = GetFiles().Count(x => x.CreationTime.Date == today);

            return new FileInformation(
                Path.Combine(Directory.FullName, $"{today:yyyyMMdd}-{(todayFilesCount + 1):D5}.log")
                );
        }
    }
}