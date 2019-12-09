using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class ConsoleLoggerWizardExtensions
    {
        #region Console

        //1
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel = LogLevel.Debug,
            IStringLogSerializer serializer = null
            )
        {
            return ConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializer
                );
        }

        //3
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            var serializerBuilder = new ConsoleLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return ConsoleLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            return ConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply
                );
        }

        //5
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            IStringLogSerializer serializer
            )
        {
            return ConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer
                );
        }

        //6
        public static ConsoleLogger ConsoleLogger(
            this LoggingWizardCharms charms,
            Action<ConsoleLogSerializerBuilder> serializerApply
            )
        {
            return ConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply
                );
        }

        #endregion Console
    }
}
