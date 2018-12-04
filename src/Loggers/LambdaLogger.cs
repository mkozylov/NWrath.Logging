using System;

namespace NWrath.Logging
{
    public class LambdaLogger
         : LoggerBase
    {
        public Action<LogRecord> WriteAction
        {
            get => _writeAction;
            set { _writeAction = value ?? throw Errors.NULL_LAMBDA; }
        }

        private Action<LogRecord> _writeAction;

        public LambdaLogger(Action<LogRecord> writeAction)
        {
            WriteAction = writeAction;
        }

        protected override void WriteRecord(LogRecord record)
        {
            _writeAction(record);
        }
    }
}