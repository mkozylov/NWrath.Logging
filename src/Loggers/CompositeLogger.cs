using System;
using System.Linq;
using System.Runtime.CompilerServices;
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
                    throw Errors.NO_LOGGERS;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            var verifiedBatch = batch.Where(r => RecordVerifier.Verify(r))
                                     .ToArray();

            if (verifiedBatch.Length == 0)
            {
                return;
            }

            var loggers = _loggers;

            foreach (var l in loggers)
            {
                l.Log(verifiedBatch);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var loggers = _loggers;

            foreach (var l in loggers)
            {
                l.Log(record);
            }
        }
    }
}