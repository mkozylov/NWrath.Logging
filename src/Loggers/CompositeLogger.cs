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
                    throw new ArgumentException(Errors.NO_LOGGERS);
                }

                _loggers = value;
            }
        }

        private ILogger[] _loggers;
        private bool _leaveOpen;

        public CompositeLogger(ILogger[] loggers, bool leaveOpen = false)
        {
            Loggers = loggers;
            _leaveOpen = leaveOpen;
        }

        ~CompositeLogger()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (!_leaveOpen)
            {
                foreach (var logger in _loggers)
                {
                    logger.Dispose();
                }
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var collection = _loggers;

            foreach (var l in collection)
            {
                l.Log(record);
            }
        }
    }
}