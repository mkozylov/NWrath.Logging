using Newtonsoft.Json.Linq;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Pipeline;
using System;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class WizardLoggingExtensions
    {
        #region File

        //1
        public static FileLogger FileLogger(
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
          this ILoggingWizardCharms charms,
          string filePath,
          ILogRecordVerifier recordVerifier,
          Action<StringLogSerializer> serializerApply,
          Encoding encoding = null,
          bool append = true
          )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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

        #region BackgroundFile

        //1
        public static BackgroundLogger BackgroundFileLogger(
           this ILoggingWizardCharms charms,
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

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundFileLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
          this ILoggingWizardCharms charms,
          string filePath,
          ILogRecordVerifier recordVerifier,
          Action<StringLogSerializer> serializerApply,
          Encoding encoding = null,
          bool append = true
          )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return BackgroundFileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append
                );
        }

        #endregion File

        #region Console

        //1
        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return ConsoleLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializer> serializerApply
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return ConsoleLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                serializerApply
                );
        }

        #endregion Console

        #region BackgroundConsole

        //1
        public static BackgroundLogger BackgroundConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            var baseLogger = new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundConsoleLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return BackgroundConsoleLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static BackgroundLogger BackgroundConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializer> serializerApply
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return BackgroundConsoleLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply
                );
        }

        #endregion Console

        #region RollingFile

        //1
        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
           string folderPath,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
           IRollingFileProvider fileProvider,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializer> serializerApply,
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

        #region BackgroundRollingFile

        //1
        public static BackgroundLogger BackgroundRollingFileLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
           string folderPath,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
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

            return BackgroundLogger(charms, baseLogger);
        }

        //8
        public static BackgroundLogger BackgroundRollingFileLogger(
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
           IRollingFileProvider fileProvider,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

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
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
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
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializer> serializerApply,
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

        #endregion RollingFile

        #region Composite

        //1
        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
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

        #region BackgroundComposite

        //1
        public static BackgroundLogger BackgroundCompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            var baseLogger = new CompositeLogger(loggers, leaveOpen)
            {
                RecordVerifier = recordVerifier
            };

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundCompositeLogger(
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
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
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
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
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, BackgroundLogger>[] loggerFactories
            )
        {
            return BackgroundCompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                loggerFactories
                );
        }

        #endregion Composite

        #region Sql

        //1
        public static SqlLogger SqlLogger(
            this ILoggingWizardCharms charms,
            SqlLogSchema schema,
            ILogRecordVerifier recordVerifier
            )
        {
            return new SqlLogger(schema)
            {
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static SqlLogger SqlLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier
           )
        {
            return SqlLogger(charms,
                new SqlLogSchema(connectionString),
                recordVerifier
                );
        }

        //3
        public static SqlLogger SqlLogger(
            this ILoggingWizardCharms charms,
            SqlLogSchema schema,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            return SqlLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static SqlLogger SqlLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return SqlLogger(
                charms, 
                connectionString, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //5
        public static SqlLogger SqlLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            var args = new SqlLogSchemaConfig();

            schemaApply(args);

            var schema = new SqlLogSchema(
                args.ConnectionString, 
                args.TableName, 
                args.InitScript, 
                args.Columns
                );

            return SqlLogger(
                charms,
                schema,
                recordVerifier
                );
        }

        //6
        public static SqlLogger SqlLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return SqlLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                schemaApply
                );
        }

        //7
        public static SqlLogger SqlLogger(
            this ILoggingWizardCharms charms,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return SqlLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                schemaApply
                );
        }

        //8
        public static SqlLogger SqlLogger(
           this ILoggingWizardCharms charms,
           SqlLogSchema schema
           )
        {
            return SqlLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Sql

        #region BackgroundSql

        //1
        public static BackgroundLogger BackgroundSqlLogger(
            this ILoggingWizardCharms charms,
            SqlLogSchema schema,
            ILogRecordVerifier recordVerifier
            )
        {
            var baseLogger = new SqlLogger(schema)
            {
                RecordVerifier = recordVerifier
            };

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundSqlLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundSqlLogger(charms,
                new SqlLogSchema(connectionString),
                recordVerifier
                );
        }

        //3
        public static BackgroundLogger BackgroundSqlLogger(
            this ILoggingWizardCharms charms,
            SqlLogSchema schema,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            return BackgroundSqlLogger(
                charms,
                schema,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static BackgroundLogger BackgroundSqlLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return BackgroundSqlLogger(
                charms,
                connectionString,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //5
        public static BackgroundLogger BackgroundSqlLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            var args = new SqlLogSchemaConfig();

            schemaApply(args);

            var schema = new SqlLogSchema(
                args.ConnectionString,
                args.TableName,
                args.InitScript,
                args.Columns
                );

            return BackgroundSqlLogger(
                charms,
                schema,
                recordVerifier
                );
        }

        //6
        public static BackgroundLogger BackgroundSqlLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return BackgroundSqlLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                schemaApply
                );
        }

        //7
        public static BackgroundLogger BackgroundSqlLogger(
            this ILoggingWizardCharms charms,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return BackgroundSqlLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                schemaApply
                );
        }

        //8
        public static BackgroundLogger BackgroundSqlLogger(
           this ILoggingWizardCharms charms,
           SqlLogSchema schema
           )
        {
            return BackgroundSqlLogger(
                charms,
                schema,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Db

        #region Debug

        //1
        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
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
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return DebugLogger(
                charms, 
                recordVerifier, 
                serializer
                );
        }

        //4
        public static DebugLogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializer> serializerApply
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
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply
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
            this ILoggingWizardCharms charms,
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

        #region BackgroundDebug

        //1
        public static BackgroundLogger BackgroundDebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            var baseLogger = new DebugLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundDebugLogger(
           this ILoggingWizardCharms charms,
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
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return BackgroundDebugLogger(
                charms,
                recordVerifier,
                serializer
                );
        }

        //4
        public static BackgroundLogger BackgroundDebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializer> serializerApply
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
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply
            )
        {
            return BackgroundDebugLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply
                );
        }

        #endregion BackgroundDebug

        #region Empty

        //1
        public static EmptyLogger EmptyLogger(
            this ILoggingWizardCharms charms
            )
        {
            return new EmptyLogger();
        }

        #endregion Empty

        #region Lambda

        //1
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
            )
        {
            return new LambdaLogger(writeAction, batchAction)
            {
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //3
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
        )
        {
            return LambdaLogger(
                charms, 
                m => batchAction(new[] { m }), 
                batchAction, 
                recordVerifier
                );
        }

        //5
        public static LambdaLogger LambdaLogger(
           this ILoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier
           )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                recordVerifier
                );
        }

        //6
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                minLevel
                );
        }

        //7
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null
                );
        }

        //8
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        //9
        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Lambda

        #region BackgroundLambda

        //1
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
            )
        {
            var baseLogger = new LambdaLogger(writeAction, batchAction)
            {
                RecordVerifier = recordVerifier
            };

            return BackgroundLogger(charms, baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                batchAction,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //3
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
        )
        {
            return BackgroundLambdaLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
        )
        {
            return BackgroundLambdaLogger(
                charms,
                m => batchAction(new[] { m }),
                batchAction,
                recordVerifier
                );
        }

        //5
        public static BackgroundLogger BackgroundLambdaLogger(
           this ILoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null,
                recordVerifier
                );
        }

        //6
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null,
                minLevel
                );
        }

        //7
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                (Action<LogRecord[]>)null
                );
        }

        //8
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction
            )
        {
            return BackgroundLambdaLogger(
                charms,
                writeAction,
                batchAction,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        //9
        public static BackgroundLogger BackgroundLambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction
        )
        {
            return BackgroundLambdaLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Lambda

        #region LoadFromJson

        //1
        public static TLogger LoadFromJson<TLogger>(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
            where TLogger : ILogger
        {
            return (TLogger)new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(filePath, sectionPath);
        }

        //2
        public static TLogger LoadFromJson<TLogger>(
           this ILoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
             where TLogger : ILogger
        {
            return (TLogger)new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(loggingSection);
        }

        //3
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
        {
            return LoadFromJson<ILogger>(charms, filePath, sectionPath, serviceProvider);
        }

        //4
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
        {
            return LoadFromJson<ILogger>(charms, loggingSection, serviceProvider);
        }

        #endregion LoadFromJson

        #region BackgroundLoadFromJson

        //1
        public static BackgroundLogger BackgroundLoadFromJson(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
        {
            var baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(filePath, sectionPath);

            var bgLogger = baseLogger as BackgroundLogger;

            return bgLogger ?? BackgroundLogger(charms, baseLogger);
        }

        //2
        public static ILogger BackgroundLoadFromJson(
           this ILoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
        {
            var baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(loggingSection);

            var bgLogger = baseLogger as BackgroundLogger;

            return bgLogger ?? BackgroundLogger(charms, baseLogger);
        }

        #endregion BackgroundLoadFromJson

        #region Background

        //1
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            ILogger logger,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return new BackgroundLogger(logger, flushPeriod, batchSize, leaveOpen)
            {
                RecordVerifier = recordVerifier,
                EmergencyLogger = emergencyLogger
            };
        }

        //2
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var lambdaLogger = LambdaLogger(charms, r => batchAction(new[] { r }), batchAction);

            return BackgroundLogger(
                charms,
                lambdaLogger,
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //3
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger[]> loggerFactories,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
           )
        {
            return BackgroundLogger(
                charms,
                CompositeLogger(charms, loggerFactories(charms)),
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //4
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger[]> loggerFactories,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return BackgroundLogger(
                charms,
                CompositeLogger(charms, loggerFactories(charms)),
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //5
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var baseLogger = loggerFactory(charms);

            return BackgroundLogger(
                charms,
                baseLogger,
                recordVerifier,
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //6
        public static BackgroundLogger BackgroundLogger(
           this ILoggingWizardCharms charms,
           ILogger logger,
           LogLevel minLevel = LogLevel.Debug,
           TimeSpan? flushPeriod = null,
           int? batchSize = null,
           bool leaveOpen = false,
           ILogger emergencyLogger = null
           )
        {
            return BackgroundLogger(
                charms,
                logger,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //7
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            var baseLogger = loggerFactory(charms);

            return BackgroundLogger(
                charms,
                baseLogger,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        //8
        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false,
            ILogger emergencyLogger = null
            )
        {
            return BackgroundLogger(
                charms,
                batchAction,
                new MinimumLogLevelVerifier(minLevel),
                flushPeriod,
                batchSize,
                leaveOpen,
                emergencyLogger
                );
        }

        #endregion Background
    }
}