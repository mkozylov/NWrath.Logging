using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundRollingFileLoggerWizardExtensions
    {
        #region BackgroundRollingFile

        //1
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                new RollingFileProvider(folderPath),
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //2
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes
                );
        }

        //3
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes
                );
        }

        //4
        public static BackgroundLogger BackgroundRollingFileLogger(
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

            return BackgroundRollingFileLogger(
                charms,
                folderPath,
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //5
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes
                );
        }

        //6
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes
                );
        }

        //7
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            var baseLogger = new RollingFileLogger(fileProvider)
            {
                RecordVerifier = recordVerifier
            };

            baseLogger.Serializer = serializer ?? baseLogger.Serializer;
            baseLogger.Encoding = encoding ?? baseLogger.Encoding;
            baseLogger.Pipes = pipes ?? baseLogger.Pipes;

            return charms.BackgroundLogger(baseLogger);
        }

        //8
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes
                );
        }

        //9
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes
                );
        }

        //10
        public static BackgroundLogger BackgroundRollingFileLogger(
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

            return BackgroundRollingFileLogger(
                charms,
                fileProvider,
                recordVerifier,
                serializer,
                encoding,
                pipes
                );
        }

        //11
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes
                );
        }

        //12
        public static BackgroundLogger BackgroundRollingFileLogger(
            this LoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializerBuilder> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return BackgroundRollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes
                );
        }

        #endregion BackgroundRollingFile
    }
}
