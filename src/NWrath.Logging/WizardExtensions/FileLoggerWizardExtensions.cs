using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class FileLoggerWizardExtensions
    {
        #region File

        //1
        public static FileLogger FileLogger(
           this LoggingWizardCharms charms,
           string filePath,
           ILogRecordVerifier recordVerifier,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           bool append = true
           )
        {
            return new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static FileLogger FileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true
            )
        {
            return FileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                append
                );
        }

        //3
        public static FileLogger FileLogger(
            this LoggingWizardCharms charms,
            string filePath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true
            )
        {
            return FileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append
                );
        }

        //4
        public static FileLogger FileLogger(
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

            return FileLogger(
               charms,
               filePath,
               recordVerifier,
               serializer,
               encoding,
               append
               );
        }

        //5
        public static FileLogger FileLogger(
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

            return FileLogger(
              charms,
              filePath,
              new MinimumLogLevelVerifier(minLevel),
              serializer,
              encoding,
              append
              );
        }

        //6
        public static FileLogger FileLogger(
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

            return FileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append
                );
        }

        #endregion File
    }
}
