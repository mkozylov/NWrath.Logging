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
           bool append = true
           )
        {
            return BackgroundLogger(charms, new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier,
                AutoFlush = false
            });
        }

        //2
        public static ILogger FileLogger(
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
        public static ILogger FileLogger(
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
        public static ILogger FileLogger(
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

            return BackgroundLogger(charms, new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier,
                AutoFlush = false
            });
        }

        //5
        public static ILogger FileLogger(
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

            return BackgroundLogger(charms, new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(minLevel),
                AutoFlush = false
            });
        }

        //6
        public static ILogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool append = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return BackgroundLogger(charms, new FileLogger(filePath, append)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug),
                AutoFlush = false
            });
        }

        #endregion File

        #region Console

        //1
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return BackgroundLogger(charms, new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            });
        }

        //2
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel = LogLevel.Debug,
            IStringLogSerializer serializer = null
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(minLevel), serializer);
        }

        //3
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return BackgroundLogger(charms, new ConsoleLogger()
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            });
        }

        //4
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(minLevel), serializerApply);
        }

        //5
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            IStringLogSerializer serializer
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializer);
        }

        //6
        public static ILogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply);
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
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            var logger = new RollingFileLogger(folderPath)
            {
                RecordVerifier = recordVerifier
            };

            logger.Serializer = serializer ?? logger.Serializer;
            logger.Encoding = encoding ?? logger.Encoding;
            logger.Pipes = pipes ?? logger.Pipes;

            return BackgroundLogger(charms, logger);
        }

        //2
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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

            return BackgroundLogger(charms, logger);
        }

        //8
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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
        public static ILogger RollingFileLogger(
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

        #region Composite

        //1
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return BackgroundLogger(charms, new CompositeLogger(loggers, leaveOpen) {
                RecordVerifier = recordVerifier
            });
        }

        //2
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, recordVerifier, false, loggers);
        }

        //3
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), leaveOpen, loggers);
        }

        //4
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), false, loggers);
        }

        //5
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), leaveOpen, loggerFactories);
        }

        //6
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), false, loggerFactories);
        }

        //7
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return CompositeLogger(charms, recordVerifier, leaveOpen, loggers);
        }

        //8
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
           )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .Select(l => l.CastAs<BackgroundLogger>()?.BaseLogger ?? l)
                                         .ToArray();

            return CompositeLogger(charms, recordVerifier, false, loggers);
        }

        //9
        public static ILogger CompositeLogger(
           this ILoggingWizardCharms charms,
           bool leaveOpen,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, LogLevel.Debug, leaveOpen, loggers);
        }

        //10
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, false, loggers);
        }

        //11
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, leaveOpen, loggerFactories);
        }

        //12
        public static ILogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, false, loggerFactories);
        }

        #endregion Composite

        #region Db

        //1
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            IDbLogSchema schema,
            ILogRecordVerifier recordVerifier
            )
        {
            return BackgroundLogger(charms, new DbLogger(schema)
            {
                RecordVerifier = recordVerifier
            });
        }

        //2
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundLogger(charms, new DbLogger(connectionString)
            {
                RecordVerifier = recordVerifier
            });
        }

        //3
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            IDbLogSchema schema,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            return DbLogger(charms, schema, new MinimumLogLevelVerifier(minLevel));
        }

        //4
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return DbLogger(charms, connectionString, new MinimumLogLevelVerifier(minLevel));
        }

        //5
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<DbLogSchemaConfig> schemaApply
            )
        {
            var args = new DbLogSchemaConfig();

            schemaApply(args);

            var schema = new SqlLogSchema(args.ConnectionString, args.TableName, args.InitScript, args.Columns);

            return BackgroundLogger(charms, new DbLogger(schema)
            {
                RecordVerifier = recordVerifier
            });
        }

        //6
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<DbLogSchemaConfig> schemaApply
            )
        {
            return DbLogger(charms, new MinimumLogLevelVerifier(minLevel), schemaApply);
        }

        //7
        public static ILogger DbLogger(
            this ILoggingWizardCharms charms,
            Action<DbLogSchemaConfig> schemaApply
            )
        {
            return DbLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), schemaApply);
        }

        //8
        public static ILogger DbLogger(
           this ILoggingWizardCharms charms,
           IDbLogSchema schema
           )
        {
            return DbLogger(charms, schema, new MinimumLogLevelVerifier(LogLevel.Debug));
        }

        #endregion Db

        #region Debug

        //1
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return BackgroundLogger(charms, new DebugLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            });
        }

        //2
        public static ILogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel = LogLevel.Debug,
           IStringLogSerializer serializer = null
           )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(minLevel), serializer);
        }

        //3
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return DebugLogger(charms, recordVerifier, serializer);
        }

        //4
        public static ILogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializer> serializerApply
           )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(minLevel), serializerApply);
        }

        //5
        public static ILogger DebugLogger(
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply
            )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply);
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
            ILogRecordVerifier recordVerifier
            )
        {
            return BackgroundLogger(charms, new LambdaLogger(writeAction, batchAction) {
                RecordVerifier = recordVerifier
            });
        }

        //2
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(charms, writeAction, batchAction, new MinimumLogLevelVerifier(minLevel));
        }

        //3
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            LogLevel minLevel
        )
        {
            return LambdaLogger(charms, batchAction, new MinimumLogLevelVerifier(minLevel));
        }

        //4
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction,
            ILogRecordVerifier recordVerifier
        )
        {
            return BackgroundLogger(charms, new LambdaLogger(batchAction) {
                RecordVerifier = recordVerifier
            });
        }

        //5
        public static ILogger LambdaLogger(
           this ILoggingWizardCharms charms,
           Action<LogRecord> writeAction,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundLogger(charms, new LambdaLogger(writeAction) {
                RecordVerifier = recordVerifier
            });
        }

        //6
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(charms, writeAction, null, minLevel);
        }

        //7
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return LambdaLogger(charms, writeAction, (Action<LogRecord[]>)null);
        }

        //8
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            Action<LogRecord[]> batchAction
            )
        {
            return LambdaLogger(charms, writeAction, batchAction, new MinimumLogLevelVerifier(LogLevel.Debug));
        }

        //9
        public static ILogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord[]> batchAction
        )
        {
            return LambdaLogger(charms, batchAction, new MinimumLogLevelVerifier(LogLevel.Debug));
        }

        #endregion Lambda

        #region LoadFromJson

        //1
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
        {
            return BackgroundLogger(charms, 
                new LoggerJsonLoader { Injector = serviceProvider }.Load(filePath, sectionPath)
                );
        }

        //2
        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
        {
            return BackgroundLogger(charms, 
                new LoggerJsonLoader { Injector = serviceProvider }.Load(loggingSection)
                );
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