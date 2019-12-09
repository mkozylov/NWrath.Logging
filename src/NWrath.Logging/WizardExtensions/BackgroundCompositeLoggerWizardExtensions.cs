using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundCompositeLoggerWizardExtensions
    {
        #region BackgroundComposite

        //1
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            var baseLogger = new CompositeLogger(loggers, leaveOpen)
            {
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundCompositeLogger(
           this LoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params BackgroundLogger[] loggers
           )
        {
            return BackgroundCompositeLogger(
                charms,
                recordVerifier,
                false,
                loggers
                );
        }

        //3
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params BackgroundLogger[] loggers
            )
        {
            return BackgroundCompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                leaveOpen,
                loggers
                );
        }

        //4
        public static BackgroundLogger BackgroundCompositeLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel,
           params BackgroundLogger[] loggers
           )
        {
            return BackgroundCompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                false,
                loggers
                );
        }

        //5
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            return BackgroundCompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                leaveOpen,
                loggerFactories
                );
        }

        //6
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            return BackgroundCompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                false,
                loggerFactories
                );
        }

        //7
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return BackgroundCompositeLogger(
                charms,
                recordVerifier,
                leaveOpen,
                loggers
                );
        }

        //8
        public static BackgroundLogger BackgroundCompositeLogger(
           this LoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
           )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return BackgroundCompositeLogger(
                charms,
                recordVerifier,
                false,
                loggers
                );
        }

        //9
        public static BackgroundLogger BackgroundCompositeLogger(
           this LoggingWizardCharms charms,
           bool leaveOpen,
           params BackgroundLogger[] loggers
           )
        {
            return BackgroundCompositeLogger(
                charms,
                LogLevel.Debug,
                leaveOpen,
                loggers
                );
        }

        //10
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            params BackgroundLogger[] loggers
            )
        {
            return BackgroundCompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                loggers
                );
        }

        //11
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            bool leaveOpen,
            params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            return BackgroundCompositeLogger(
                charms,
                LogLevel.Debug,
                leaveOpen,
                loggerFactories
                );
        }

        //12
        public static BackgroundLogger BackgroundCompositeLogger(
            this LoggingWizardCharms charms,
            params Func<LoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            return BackgroundCompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                loggerFactories
                );
        }

        #endregion BackgroundComposite
    }
}
