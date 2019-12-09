using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class DebugLoggerWizardExtensions
    {
        #region Debug

        //1
        public static DebugLogger DebugLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return new DebugLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static DebugLogger DebugLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel = LogLevel.Debug,
           IStringLogSerializer serializer = null
           )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializer
                );
        }

        //3
        public static DebugLogger DebugLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializerBuilder> serializerApply
            )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return DebugLogger(
                charms, 
                recordVerifier, 
                serializer
                );
        }

        //4
        public static DebugLogger DebugLogger(
           this LoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializerBuilder> serializerApply
           )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializerApply
                );
        }

        //5
        public static DebugLogger DebugLogger(
            this LoggingWizardCharms charms,
            Action<StringLogSerializerBuilder> serializerApply
            )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                serializerApply
                );
        }

        //6
        public static DebugLogger DebugLogger(
            this LoggingWizardCharms charms,
            IStringLogSerializer serializer
            )
        {
            return DebugLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer
                );
        }

        #endregion Debug

    }
}
