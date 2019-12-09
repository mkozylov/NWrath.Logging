using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class LambdaLoggerWizardExtensions
    {
        #region Lambda

        //1
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
            )
        {
            return new LambdaLogger(writeAction, batchAction)
            {
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //3
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
        )
        {
            return LambdaLogger(
                charms, 
                m => batchAction(new[] { m }), 
                batchAction, 
                recordVerifier
                );
        }

        //5
        public static LambdaLogger LambdaLogger(
           this LoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier
           )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                recordVerifier
                );
        }

        //6
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                minLevel
                );
        }

        //7
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null
                );
        }

        //8
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        //9
        public static LambdaLogger LambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Lambda

    }
}
