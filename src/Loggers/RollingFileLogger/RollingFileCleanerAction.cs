using NWrath.Synergy.Common;
using System;
using System.IO;
using System.Linq;

namespace NWrath.Logging
{
    public class RollingFileCleanerAction
        : IRollingFileAction
    {
        public TimeSpan ExpirationTimeSpan { get; set; }

        public RollingFileCleanerAction(TimeSpan expirationTimeSpan)
        {
            ExpirationTimeSpan = expirationTimeSpan;
        }

        public void Execute(RollingFileContext ctx)
        {
            var expireTime = Clock.Now - ExpirationTimeSpan;

            var expiredFiles = ctx.FileProvider.GetFiles()
                                  .Select(x => new FileInfo(x))
                                  .Where(x => x.CreationTime <= expireTime)
                                  .ToArray();

            var currentExpiredFile = "";

            foreach (var file in expiredFiles)
            {
                if (file.FullName == ctx.LogFile.Path)
                {
                    currentExpiredFile = file.FullName;
                }
                else
                {
                    file.Delete();
                }
            }

            if (!string.IsNullOrEmpty(currentExpiredFile))
            {
                ctx.LogFile.Change(ctx.FileProvider.ProduceNewFile());
                File.Delete(currentExpiredFile);
            }
        }
    }
}