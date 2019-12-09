using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundLambdaLoggerWizardExtensions
    {
        #region BackgroundLambda

        //1
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
            )
        {
            var baseLogger = new LambdaLogger(writeAction, batchAction)
            {
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                batchAction,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //3
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
        )
        {
            return BackgroundLambdaLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
        )
        {
            return BackgroundLambdaLogger(
                charms,
                m => batchAction(new[] { m }),
                batchAction,
                recordVerifier
                );
        }

        //5
        public static BackgroundLogger BackgroundLambdaLogger(
           this LoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null,
                recordVerifier
                );
        }

        //6
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null,
                minLevel
                );
        }

        //7
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null
                );
        }

        //8
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                batchAction,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        //9
        public static BackgroundLogger BackgroundLambdaLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction
        )
        {
            return BackgroundLambdaLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Lambda

    }
}
