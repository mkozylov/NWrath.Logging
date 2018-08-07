using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace NWrath.Logging
{
    public class ThreadSaveLogger
        : LoggerBase
    {
        public override bool IsEnabled
        {
            get => logger.IsEnabled;
            set => logger.IsEnabled = value;
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => logger.LevelVerifier;
            set => logger.LevelVerifier = value;
        }

        protected ILogger logger;
        private object _thisLock = new object();

        public ThreadSaveLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public override void Log(LogMessage log)
        {
            WriteLog(log);
        }

        protected override void WriteLog(LogMessage log)
        {
            lock (_thisLock)
            {
                logger.Log(log);
            }
        }
    }
}