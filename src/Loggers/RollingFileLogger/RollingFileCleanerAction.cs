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
                                  .Where(x => x.Exists && x.CreationTime <= expireTime)
                                  .ToArray();

            foreach (var file in expiredFiles)
            {
                if (file.FullName == ctx.LogFile.Path)
                {
                    var newFile = ctx.FileProvider.ProduceNewFile();

                    ctx.LogFile.Change(newFile.FullName);
                }

                file.Delete();
            }
        }
    }
}