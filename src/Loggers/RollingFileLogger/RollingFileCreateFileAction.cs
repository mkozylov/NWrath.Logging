using NWrath.Synergy.Common;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class RollingFileCreateFileAction
        : IRollingFileAction
    {
        private bool _append;
        private Lazy<bool> _tryUseTodayLastFile;

        public RollingFileCreateFileAction(
            bool append = false,
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
                          ? GetTodayLastFileOrCreateNew(ctx.Logger.FileProvider)
                          : ctx.Logger.FileProvider.ProduceNewFile();

            ctx.Logger.Writer = new Lazy<IFileLogger>(() => new FileLogger(newFile, append: _append));
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