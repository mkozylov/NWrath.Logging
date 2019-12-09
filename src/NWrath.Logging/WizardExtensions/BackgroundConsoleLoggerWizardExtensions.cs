using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundConsoleLoggerWizardExtensions
    {
        #region BackgroundConsole

        //1
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            var baseLogger = new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel = LogLevel.Debug,
            IStringLogSerializer serializer = null
            )
        {
            return BackgroundConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializer
                );
        }

        //3
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            var serializerBuilder = new ConsoleLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return BackgroundConsoleLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            return BackgroundConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply
                );
        }

        //5
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            IStringLogSerializer serializer
            )
        {
            return BackgroundConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer
                );
        }

        //6
        public static BackgroundLogger BackgroundConsoleLogger(
            this LoggingWizardCharms charms,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            return BackgroundConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply
                );
        }

        #endregion BackgroundConsole
    }
}
