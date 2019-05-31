using NWrath.Synergy.Common.Structs;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public abstract class LoggerBase
        : ILogger
    {
        public virtual ILogRecordVerifier RecordVerifier
        {
            get => _recordVerifier;
            set => _recordVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug);
        }

        public virtual bool IsEnabled { get; set; } = true;

        private ILogRecordVerifier _recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Log(LogRecord record)
        {
            if (IsEnabled && RecordVerifier.Verify(record))
            {
                WriteRecord(record);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            foreach (var record in batch)
            {
                if (RecordVerifier.Verify(record))
                {
                    WriteRecord(record);
                }
            }
        }

        public virtual void Dispose()
        {
        }

        protected abstract void WriteRecord(LogRecord record);
    }
}