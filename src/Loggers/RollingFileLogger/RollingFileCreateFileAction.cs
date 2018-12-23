using NWrath.Synergy.Common;
using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;

namespace NWrath.Logging
{
    public class RollingFileCreateFileAction
        : IRollingFileAction
    {
        private FileMode _fileMode;
        private Lazy<bool> _tryUseTodayLastFile;

        public RollingFileCreateFileAction(
            FileMode fileMode,
            bool tryUseTodayLastFile = true
            )
        {
            _fileMode = fileMode;
            _tryUseTodayLastFile = new Lazy<bool>(() => tryUseTodayLastFile);
        }

        public void Execute(RollingFileContext ctx)
        {
            var needUseToday = (!_tryUseTodayLastFile.IsValueCreated && _tryUseTodayLastFile.Value);

            var newFile = needUseToday
                          ? GetTodayLastFileOrCreateNew(ctx.FileProvider)
                          : ctx.FileProvider.ProduceNewFile();

            ctx.LogFile.Change(newFile, _fileMode);
        }

        private string GetTodayLastFileOrCreateNew(IRollingFileProvider fileNameProvider)
        {
            var fileName = fileNameProvider.TryResolveLastFile();

            if (fileName.IsEmpty()
                || new FileInfo(fileName).CreationTime.Date != Clock.Today)
            {
                fileName = fileNameProvider.ProduceNewFile();
            }

            return fileName;
        }
    }
}