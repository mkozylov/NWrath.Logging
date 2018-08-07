using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public class CompositeLogger
         : LoggerBase
    {
        public virtual ILogger[] Loggers
        {
            get => loggers;

            set
            {
                if (value.IsEmpty())
                {
                    throw new Exception(Errors.NO_LOGGERS);
                }

                loggers = value;
            }
        }

        protected ILogger[] loggers;

        public CompositeLogger(ILogger[] loggers)
        {
            Loggers = loggers;
        }

        protected override void WriteLog(LogMessage log)
        {
            var collection = loggers;

            foreach (var l in collection)
            {
                l.Log(log);
            }
        }
    }
}