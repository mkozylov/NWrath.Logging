using NWrath.Synergy.Common;
using System;

namespace NWrath.Logging
{
    public class RollingFileCreateFileAction
        : IRollingFileAction
    {
        private bool _append;
        private Lazy<bool> _tryUseTodayLastFile;

        public RollingFileCreateFileAction(
            bool append,
            bool tryUseTodayLastFile = true
            )
        {
            _append = append;
            _tryUseTodayLastFile = new Lazy<bool>(() => tryUseTodayLastFile);
        }

        public void Execute(RollingFileContext ctx)
        {
            var needUseToday = (!_tryUseTodayLastFile.IsValueCreated && _tryUseTodayLastFile.Value);

            var newFile = needUseToday
                          ? GetTodayLastFileOrCreateNew(ctx.FileProvider)
                          : ctx.FileProvider.ProduceNewFile();

            ctx.LogFile.Change(newFile.FullName, _append);
        }

        private FileInformation GetTodayLastFileOrCreateNew(IRollingFileProvider fileNameProvider)
        {
            var file = fileNameProvider.TryResolveLastFile();

            if (file == null
                || !file.Exists
                || file.CreationTime.Date != Clock.Today)
            {
                file = fileNameProvider.ProduceNewFile();
            }

            return file;
        }
    }
}