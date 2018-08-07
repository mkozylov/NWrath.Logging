using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace NWrath.Logging
{
    public class EmptyLogger
         : LoggerBase
    {
        public override void Log(LogMessage log)
        {
            WriteLog(log);
        }

        protected override void WriteLog(LogMessage log)
        {
        }
    }
}