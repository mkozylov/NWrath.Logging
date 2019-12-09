using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundLoggerWizardExtensions
    {
        #region Background

        //1
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            ILogger logger,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return new BackgroundLogger(logger, flushPeriod, batchSize, leaveOpen)
            {
                RecordVerifier = recordVerifier,
                EmergencyLogger = emergencyLogger
            };
        }

        //2
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var lambdaLogger = charms.LambdaLogger(r => batchAction(new[] { r }), batchAction);

            return BackgroundLogger(
                charms,
                lambdaLogger,
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //3
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Func<LoggingWizardCharms, ILogger[]> loggerFactories,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
           )
        {
            return BackgroundLogger(
                charms,
                charms.CompositeLogger(loggerFactories(charms)),
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //4
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Func<LoggingWizardCharms, ILogger[]> loggerFactories,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return BackgroundLogger(
                charms,
                charms.CompositeLogger(loggerFactories(charms)),
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //5
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Func<LoggingWizardCharms, ILogger> loggerFactory,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var baseLogger = loggerFactory(charms);

            return BackgroundLogger(
                charms,
                baseLogger,
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //6
        public static BackgroundLogger BackgroundLogger(
           this LoggingWizardCharms charms,
           ILogger logger,
           LogLevel minLevel = LogLevel.Debug,
           TimeSpan? flushPeriod = null,
           int? batchSize = null,
           bool leaveOpen = false,
           ILogger emergencyLogger = null
           )
        {
            return BackgroundLogger(
                charms,
                logger,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //7
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Func<LoggingWizardCharms, ILogger> loggerFactory,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var baseLogger = loggerFactory(charms);

            return BackgroundLogger(
                charms,
                baseLogger,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //8
        public static BackgroundLogger BackgroundLogger(
            this LoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return BackgroundLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        #endregion Background

    }
}
