﻿using System;

namespace NWrath.Logging
{
    public class LambdaLogger
         : LoggerBase
    {
        public Action<LogMessage> WriteAction { get; set; }

        public LambdaLogger(Action<LogMessage> writeAction)
        {
            WriteAction = writeAction;
        }

        protected override void WriteLog(LogMessage log)
        {
            WriteAction(log);
        }
    }
}