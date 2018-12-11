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

            var expiredFiles = ctx.Logger.FileProvider.GetFiles()
                                  .Select(x => new FileInfo(x))
                                  .Where(x => x.CreationTime <= expireTime)
                                  .ToArray();

            var currentExpiredFile = "";

            foreach (var file in expiredFiles)
            {
                if (ctx.Logger.Writer.IsValueCreated
                    && file.FullName == ctx.Logger.Writer.Value.FilePath)
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
                ctx.Logger.Writer.Value.FilePath = ctx.Logger.FileProvider.ProduceNewFile();
                File.Delete(currentExpiredFile);
            }
        }
    }
}