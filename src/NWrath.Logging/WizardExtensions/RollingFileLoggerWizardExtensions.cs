using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class RollingFileLoggerWizardExtensions
    {
        #region RollingFile

        //1
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                new RollingFileProvider(folderPath),
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //2
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes
                );
        }

        //3
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes
                );
        }

        //4
        public static RollingFileLogger RollingFileLogger(
           this LoggingWizardCharms charms,
           string folderPath,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializerBuilder> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return RollingFileLogger(
                charms,
                folderPath,
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //5
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes
                );
        }

        //6
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes
                );
        }

        //7
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            var logger = new RollingFileLogger(fileProvider)
            {
                RecordVerifier = recordVerifier
            };

            logger.Serializer = serializer ?? logger.Serializer;
            logger.Encoding = encoding ?? logger.Encoding;
            logger.Pipes = pipes ?? logger.Pipes;

            return logger;
        }

        //8
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes
                );
        }

        //9
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes
                );
        }

        //10
        public static RollingFileLogger RollingFileLogger(
           this LoggingWizardCharms charms,
           IRollingFileProvider fileProvider,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializerBuilder> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            serializerApply?.Invoke(serializerBuilder);

            var serializer = serializerBuilder.BuildSerializer();

            return RollingFileLogger(
                charms,
                fileProvider,
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //11
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes
                );
        }

        //12
        public static RollingFileLogger RollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes
                );
        }

        #endregion RollingFile
    }
}
