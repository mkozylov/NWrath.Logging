using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class RollingFileDisableLoggerAction
        : IRollingFileAction
    {
        public void Execute(RollingFileContext ctx)
        {
            ctx.Logger.IsEnabled = false;
        }
    }
}