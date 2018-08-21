using System;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public class CompositeLogger
         : LoggerBase
    {
        public ILogger[] Loggers
        {
            get => _loggers;

            set
            {
                if (value.IsEmpty())
                {
                    throw new Exception(Errors.NO_LOGGERS);
                }

                _loggers = value;
            }
        }

        private ILogger[] _loggers;

        public CompositeLogger(ILogger[] loggers)
        {
            Loggers = loggers;
        }

        protected override void WriteLog(LogMessage log)
        {
            var collection = _loggers;

            foreach (var l in collection)
            {
                l.Log(log);
            }
        }
    }
}