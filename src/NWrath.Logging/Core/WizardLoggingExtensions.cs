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
        public static ILogger FileLogger(
           this ILoggingWizardCharms charms,
           string filePath,
           ILogRecordVerifier recordVerifier,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           bool append = true,
           bool background = true
           )
        {
            ILogger baseLogger = new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier,
                AutoFlush = !background
            };

            return background
                ? BackgroundLogger(charms, baseLogger)
                : baseLogger;
        }

        //2
        public static ILogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true,
            bool background = true
            )
        {
            return FileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                append,
                background
                );
        }

        //3
        public static ILogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool append = true,
            bool background = true
            )
        {
            return FileLogger(
                charms,
                filePath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                append,
                background
                );
        }

        //4
        public static ILogger FileLogger(
          this ILoggingWizardCharms charms,
          string filePath,
          ILogRecordVerifier recordVerifier,
          Action<StringLogSerializer> serializerApply,
          Encoding encoding = null,
          bool append = true,
          bool background = true
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
               append,
               background
               );
        }

        //5
        public static ILogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true,
            bool background = true
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
              append,
              background
              );
        }

        //6
        public static ILogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true,
            bool background = true
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
                append,
                background
                );
        }

        #endregion File

        #region Console

        //1
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            bool background = true
            )
        {
            ILogger baseLogger = new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return background
                ? BackgroundLogger(charms, baseLogger)
                : baseLogger;
        }

        //2
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel = LogLevel.Debug,
            IStringLogSerializer serializer = null,
            bool background = true
            )
        {
            return ConsoleLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializer, 
                background
                );
        }

        //3
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializer> serializerApply,
            bool background = true
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return ConsoleLogger(
                charms,
                recordVerifier,
                serializer,
                background
                );
        }

        //4
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializer> serializerApply,
            bool background = true
            )
        {
            return ConsoleLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializerApply, 
                background
                );
        }

        //5
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            IStringLogSerializer serializer,
            bool background = true
            )
        {
            return ConsoleLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                serializer,
                background
                );
        }

        //6
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply,
            bool background = true
            )
        {
            return ConsoleLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                serializerApply,
                background
                );
        }

        #endregion Console

        #region RollingFile

        //1
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                new RollingFileProvider(folderPath),
                recordVerifier,
                serializer,
                encoding,
                pipes,
                background
                );
        }

        //2
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes,
                background
                );
        }

        //3
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes,
                background
                );
        }

        //4
        public static ILogger RollingFileLogger(
           this ILoggingWizardCharms charms,
           string folderPath,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null,
           bool background = true
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
                pipes,
                background
                );
        }

        //5
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes,
                background
                );
        }

        //6
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                folderPath,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes,
                background
                );
        }

        //7
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            var baseLogger = new RollingFileLogger(fileProvider)
            {
                RecordVerifier = recordVerifier
            };

            baseLogger.Serializer = serializer ?? baseLogger.Serializer;
            baseLogger.Encoding = encoding ?? baseLogger.Encoding;
            baseLogger.Pipes = pipes ?? baseLogger.Pipes;

            return background
                ? BackgroundLogger(charms, baseLogger)
                : (ILogger)baseLogger;
        }

        //8
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializer,
                encoding,
                pipes,
                background
                );
        }

        //9
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializer,
                encoding,
                pipes,
                background
                );
        }

        //10
        public static ILogger RollingFileLogger(
           this ILoggingWizardCharms charms,
           IRollingFileProvider fileProvider,
           ILogRecordVerifier recordVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null,
           bool background = true
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
                pipes,
                background
                );
        }

        //11
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(minLevel),
                serializerApply,
                encoding,
                pipes,
                background
                );
        }

        //12
        public static ILogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null,
            bool background = true
            )
        {
            return RollingFileLogger(
                charms,
                fileProvider,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                serializerApply,
                encoding,
                pipes,
                background
                );
        }

        #endregion RollingFile

        #region Composite

        //1
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            bool background = true,
            params ILogger[] loggers
            )
        {
            ILogger baseLogger = new CompositeLogger(loggers, leaveOpen)
            {
                RecordVerifier = recordVerifier
            };

            return background 
                ? BackgroundLogger(charms, baseLogger) 
                : baseLogger;
        }

        //2
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           bool background,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms, 
                recordVerifier, 
                false, 
                background, 
                loggers
                );
        }

        //3
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms,
                recordVerifier,
                false,
                true,
                loggers
                );
        }

        //4
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            bool background = true,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                leaveOpen, 
                background,
                loggers
                );
        }

        //5
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           bool background,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                false, 
                background, 
                loggers
                );
        }

        //6
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                false,
                true,
                loggers
                );
        }

        //7
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            bool background = true,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                leaveOpen,
                background,
                loggerFactories
                );
        }

        //8
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool background,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                false, 
                background,
                loggerFactories
                );
        }

        //9
        public static ILogger CompositeLogger(
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

        //10
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            bool background = true,
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
                background, 
                loggers
                );
        }

        //11
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           bool background,
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
                background, 
                loggers
                );
        }

        //12
        public static ILogger CompositeLogger(
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
                true,
                loggers
                );
        }

        //13
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           bool leaveOpen,
           bool background = true,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(
                charms, 
                LogLevel.Debug, 
                leaveOpen, 
                background, 
                loggers
                );
        }

        //14
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            bool background,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(
                charms, 
                LogLevel.Debug, 
                false, 
                background, 
                loggers
                );
        }

        //15
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                true,
                loggers
                );
        }

        //16
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            bool leaveOpen,
            bool background = true,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms, 
                LogLevel.Debug, 
                leaveOpen, 
                background, 
                loggerFactories
                );
        }

        //17
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            bool background,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms, 
                LogLevel.Debug, 
                false, 
                background, 
                loggerFactories
                );
        }

        //18
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(
                charms,
                LogLevel.Debug,
                false,
                true,
                loggerFactories
                );
        }

        #endregion Composite

        #region Db

        //1
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            IDbLogSchema schema,
            ILogRecordVerifier recordVerifier,
            bool background = true
            )
        {
            ILogger baseLogger = new DbLogger(schema)
            {
                RecordVerifier = recordVerifier
            };

            return background 
                ? BackgroundLogger(charms, baseLogger) 
                : baseLogger;
        }

        //2
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier,
           bool background = true
           )
        {
            return DbLogger(charms,
                new SqlLogSchema(connectionString),
                recordVerifier,
                background
                );
        }

        //3
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            IDbLogSchema schema,
            LogLevel minLevel = LogLevel.Debug,
            bool background = true
            )
        {
            return DbLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(minLevel), 
                background
                );
        }

        //4
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug,
           bool background = true
           )
        {
            return DbLogger(
                charms, 
                connectionString, 
                new MinimumLogLevelVerifier(minLevel),
                background
                );
        }

        //5
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<DbLogSchemaConfig> schemaApply,
            bool background = true
            )
        {
            var args = new DbLogSchemaConfig();

            schemaApply(args);

            var schema = new SqlLogSchema(args.ConnectionString, args.TableName, args.InitScript, args.Columns);

            return DbLogger(
                charms,
                schema,
                recordVerifier,
                background
                );
        }

        //6
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<DbLogSchemaConfig> schemaApply,
            bool background = true
            )
        {
            return DbLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                schemaApply, 
                background
                );
        }

        //7
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            Action<DbLogSchemaConfig> schemaApply,
            bool background = true
            )
        {
            return DbLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                schemaApply,
                background
                );
        }

        //8
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           IDbLogSchema schema,
           bool background = true
           )
        {
            return DbLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(LogLevel.Debug),
                background
                );
        }

        #endregion Db

        #region Debug

        //1
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            bool background = true
            )
        {
            ILogger baseLogger = new DebugLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };

            return background
                ? BackgroundLogger(charms, baseLogger) 
                : baseLogger;
        }

        //2
        public static ILogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel = LogLevel.Debug,
           IStringLogSerializer serializer = null,
           bool background = true
           )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializer, 
                background
                );
        }

        //3
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply,
            bool background = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return DebugLogger(
                charms, 
                recordVerifier, 
                serializer, 
                background
                );
        }

        //4
        public static ILogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializer> serializerApply,
           bool background = true
           )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                serializerApply, 
                background
                );
        }

        //5
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply,
            bool background = true
            )
        {
            return DebugLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                serializerApply, 
                background
                );
        }

        #endregion Debug

        #region Empty

        //1
        public static ILogger EmptyLogger(
            this ILoggingWizardCharms charms
            )
        {
            return new EmptyLogger();
        }

        #endregion Empty

        #region Lambda

        //1
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier,
            bool background = true
            )
        {
            ILogger baseLogger = new LambdaLogger(writeAction, batchAction)
            {
                RecordVerifier = recordVerifier
            };

            return background 
                ? BackgroundLogger(charms, baseLogger) 
                : baseLogger;
        }

        //2
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel,
            bool background = true
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel), 
                background
                );
        }

        //3
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel,
            bool background = true
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(minLevel), 
                background
                );
        }

        //4
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier,
            bool background = true
        )
        {
            return LambdaLogger(
                charms, 
                m => batchAction(new[] { m }), 
                batchAction, 
                recordVerifier, 
                background
                );
        }

        //5
        public static ILogger LambdaLogger(
           this ILoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier,
           bool background = true
           )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                recordVerifier, 
                background
                );
        }

        //6
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel,
            bool background = true
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                minLevel, 
                background
                );
        }

        //7
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
             bool background = true
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                (Action<LogRecord[]>)null, 
                background
                );
        }

        //8
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
             bool background = true
            )
        {
            return LambdaLogger(
                charms, 
                writeAction, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                background
                );
        }

        //9
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            bool background = true
        )
        {
            return LambdaLogger(
                charms, 
                batchAction, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                background
                );
        }

        #endregion Lambda

        #region LoadFromJson

        //1
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null,
           bool background = true
           )
        {
            ILogger baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(filePath, sectionPath);

            return background && !(baseLogger is BackgroundLogger)
                ? BackgroundLogger(charms, baseLogger) 
                : baseLogger;
        }

        //2
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null,
           bool background = true
           )
        {
            ILogger baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(loggingSection);

            return background && !(baseLogger is BackgroundLogger)
                ? BackgroundLogger(charms, baseLogger)
                : baseLogger;
        }

        #endregion LoadFromJson

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
                CompositeLogger(charms, background: false, loggerFactories(charms)),
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
                CompositeLogger(charms, background: false, loggerFactories(charms)),
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