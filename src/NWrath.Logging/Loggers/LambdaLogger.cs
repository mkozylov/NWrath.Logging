using System;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public class LambdaLogger
         : LoggerBase
    {
        public Action<LogRecord> WriteAction
        {
            get => _writeAction;
            set => _writeAction = value ?? throw Errors.NULL_LAMBDA;
        }

        public Action<LogRecord[]> BatchAction
        {
            get => _batchAction;
            set => _batchAction = value ?? throw Errors.NULL_LAMBDA;
        }

        private Action<LogRecord> _writeAction;
        private Action<LogRecord[]> _batchAction;

        public LambdaLogger(Action<LogRecord> writeAction, Action<LogRecord[]> batchAction = null)
        {
            WriteAction = writeAction;
            BatchAction = batchAction ?? DefaultBatchLog;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            BatchAction(batch);
        }

        protected override void WriteRecord(LogRecord record)
        {
            _writeAction(record);
        }

        private void DefaultBatchLog(LogRecord[] batch)
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
    }
}