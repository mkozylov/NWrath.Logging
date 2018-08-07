using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public interface IRollingFileAction
    {
        void Execute(RollingFileContext ctx);
    }
}