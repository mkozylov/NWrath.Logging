using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundFileLoggerWizardExtensions
    {
        #region BackgroundFile

        //1
        public static BackgroundLogger BackgroundFileLogger(
           this LoggingWizardCharms charms,
           string filePath,
           ILogRecordVerifier recordVerifier,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           bool append = true
           )
        {
            var baseLogger = new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundFileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true
            )
        {
            return BackgroundFileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                append
                );
        }

        //3
        public static BackgroundLogger BackgroundFileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true
            )
        {
            return BackgroundFileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append
                );
        }

        //4
        public static BackgroundLogger BackgroundFileLogger(
          this LoggingWizardCharms charms,
          string filePath,
          ILogRecordVerifier recordVerifier,
          Action<StringLogSerializerBuilder> serializerApply,
          Encoding encoding = null,
          bool append = true
          )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return BackgroundFileLogger(
               charms,
               filePath,
               recordVerifier,
               serializer,
               encoding,
               append
               );
        }

        //5
        public static BackgroundLogger BackgroundFileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return BackgroundFileLogger(
              charms,
              filePath,
              new MinimumLogLevelVerifier(minLevel),
              serializer,
              encoding,
              append
              );
        }

        //6
        public static BackgroundLogger BackgroundFileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return BackgroundFileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append
                );
        }

        #endregion BackgroundFile
    }
}
