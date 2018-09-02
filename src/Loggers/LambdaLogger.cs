using System;

namespace NWrath.Logging
{
    public class LambdaLogger
         : LoggerBase
    {
        public Action<LogMessage> WriteAction { get => _writeAction; set { _writeAction = value ?? throw new ArgumentNullException(Errors.NULL_LAMBDA); } }

        private Action<LogMessage> _writeAction;

        public LambdaLogger(Action<LogMessage> writeAction)
        {
            WriteAction = writeAction;
        }

        protected override void WriteLog(LogMessage log)
        {
            _writeAction(log);
        }
    }
}