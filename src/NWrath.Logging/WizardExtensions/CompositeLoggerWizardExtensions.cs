using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class CompositeLoggerWizardExtensions
    {
        #region Composite

        //1
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return new CompositeLogger(loggers, leaveOpen)
            {
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static CompositeLogger CompositeLogger(
           this LoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms,
                recordVerifier,
                false,
                loggers
                );
        }

        //3
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                leaveOpen,
                loggers
                );
        }

        //4
        public static CompositeLogger CompositeLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                false,
                loggers
                );
        }

        //5
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<LoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                leaveOpen,
                loggerFactories
                );
        }

        //6
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<LoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                false,
                loggerFactories
                );
        }

        //7
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<LoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return CompositeLogger(
                charms,
                recordVerifier,
                leaveOpen,
                loggers
                );
        }

        //8
        public static CompositeLogger CompositeLogger(
           this LoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<LoggingWizardCharms, ILogger>[] loggerFactories
           )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return CompositeLogger(
                charms,
                recordVerifier,
                false,
                loggers
                );
        }

        //9
        public static CompositeLogger CompositeLogger(
           this LoggingWizardCharms charms,
           bool leaveOpen,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                leaveOpen,
                loggers
                );
        }

        //10
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                loggers
                );
        }

        //11
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            bool leaveOpen,
            params Func<LoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                leaveOpen,
                loggerFactories
                );
        }

        //12
        public static CompositeLogger CompositeLogger(
            this LoggingWizardCharms charms,
            params Func<LoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                loggerFactories
                );
        }

        #endregion Composite
    }
}
