using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundDebugLoggerWizardExtensions
    {
        #region BackgroundDebug

        //1
        public static BackgroundLogger BackgroundDebugLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            var baseLogger = new DebugLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundDebugLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel = LogLevel.Debug,
           IStringLogSerializer serializer = null
           )
        {
            return BackgroundDebugLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializer
                );
        }

        //3
        public static BackgroundLogger BackgroundDebugLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializerBuilder> serializerApply
            )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return BackgroundDebugLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static BackgroundLogger BackgroundDebugLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializerBuilder> serializerApply
           )
        {
            return BackgroundDebugLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply
                );
        }

        //5
        public static BackgroundLogger BackgroundDebugLogger(
            this LoggingWizardCharms charms,
            Action<StringLogSerializerBuilder> serializerApply
            )
        {
            return BackgroundDebugLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply
                );
        }

        #endregion BackgroundDebug

    }
}
