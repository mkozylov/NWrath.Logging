using System;
using System.Text;
using NWrath.Synergy.Pipeline;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public static class WizardLoggingExtensions
    {
        #region File

        public static FileLogger FileLogger(
           this ILoggingWizardCharms charms,
           string filePath,
           ILogRecordVerifier recordVerifier,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           FileMode fileMode = FileMode.Append,
           bool autoFlush = true
           )
        {
            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier,
                FileMode = fileMode,
                AutoFlush = autoFlush
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append,
            bool autoFlush = true
            )
        {
            return FileLogger(charms, filePath, new MinimumLogLevelVerifier(minLevel), serializer, encoding, fileMode, autoFlush);
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append,
            bool autoFlush = true
            )
        {
            return FileLogger(charms, filePath, new MinimumLogLevelVerifier(LogLevel.Debug), serializer, encoding, fileMode, autoFlush);
        }

        public static FileLogger FileLogger(
          this ILoggingWizardCharms charms,
          string filePath,
          ILogRecordVerifier recordVerifier,
          Action<StringLogSerializer> serializerApply,
          Encoding encoding = null,
          FileMode fileMode = FileMode.Append,
          bool autoFlush = true
          )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier,
                FileMode = fileMode,
                AutoFlush = autoFlush
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append,
            bool autoFlush = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(minLevel),
                FileMode = fileMode,
                AutoFlush = autoFlush
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append,
            bool autoFlush = true
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug),
                FileMode = fileMode,
                AutoFlush = autoFlush
            };
        }

        #endregion File

        #region Console

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

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel = LogLevel.Debug,
            IStringLogSerializer serializer = null
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(minLevel), serializer);
        }

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return new ConsoleLogger
            {
                Serializer = serializer,
                RecordVerifier = recordVerifier
            };
        }

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(minLevel), serializerApply);
        }

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            IStringLogSerializer serializer
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializer);
        }

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            return ConsoleLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply);
        }

        #endregion Console

        #region Pipe

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            collection.AddRange(pipes);

            return PipeLogger(charms, logger, collection, null);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            Set properties = null,
            bool leaveOpen = false,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            collection.AddRange(pipes);

            return PipeLogger(charms, logger, collection, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger
        {
            return new PipeLogger<TLogger>(logger, leaveOpen) { Pipes = pipes, Properties = properties };
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           TLogger logger,
           IPipe<PipeLoggerContext<TLogger>>[] pipes,
           Set properties = null,
           bool leaveOpen = false
           )
           where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            collection.AddRange(pipes);

            return PipeLogger(charms, logger, collection, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            pipesApply(collection);

            return PipeLogger(charms, logger, collection, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           Func<ILoggingWizardCharms, TLogger> loggerFactory,
           IPipe<PipeLoggerContext<TLogger>>[] pipes,
           Set properties = null,
           bool leaveOpen = false
           )
           where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, null);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            Set properties = null,
            bool leaveOpen = false,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipesApply, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            IPipe<PipeLoggerContext<TLogger>>[] pipes,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, null);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Set properties = null,
            bool leaveOpen = false,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties, leaveOpen);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null,
            bool leaveOpen = false
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipesApply, properties, leaveOpen);
        }

        #endregion Pipe

        #region RollingFile

        public static RollingFileLogger RollingFileLogger(
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

            return logger;
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, folderPath, new MinimumLogLevelVerifier(minLevel), serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, folderPath, new MinimumLogLevelVerifier(LogLevel.Debug), serializer, encoding, pipes);
        }

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

            return RollingFileLogger(charms, folderPath, recordVerifier, serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, folderPath, new MinimumLogLevelVerifier(minLevel), serializerApply, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, folderPath, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply, encoding, pipes);
        }

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

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, fileProvider, new MinimumLogLevelVerifier(minLevel), serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, fileProvider, new MinimumLogLevelVerifier(LogLevel.Debug), serializer, encoding, pipes);
        }

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

            return RollingFileLogger(charms, fileProvider, recordVerifier, serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, fileProvider, new MinimumLogLevelVerifier(minLevel), serializerApply, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileProvider,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return RollingFileLogger(charms, fileProvider, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply, encoding, pipes);
        }

        #endregion RollingFile

        #region Composite

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return new CompositeLogger(loggers, leaveOpen) { RecordVerifier = recordVerifier };
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, recordVerifier, false, loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), leaveOpen, loggers);
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), false, loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), leaveOpen, loggerFactories);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), false, loggerFactories);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .ToArray();

            return CompositeLogger(charms, recordVerifier, leaveOpen, loggers);
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogRecordVerifier recordVerifier,
           params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
           )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .ToArray();

            return CompositeLogger(charms, recordVerifier, false, loggers);
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           bool leaveOpen,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, LogLevel.Debug, leaveOpen, loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, false, loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            bool leaveOpen,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, leaveOpen, loggerFactories);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, false, loggerFactories);
        }

        #endregion Composite

        #region Db

        public static DbLogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier,
           ILogTableSchema tableSchema = null
           )
        {
            return new DbLogger(connectionString)
            {
                TableSchema = tableSchema,
                RecordVerifier = recordVerifier
            };
        }

        public static DbLogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug,
           ILogTableSchema tableSchema = null
           )
        {
            return DbLogger(charms, connectionString, new MinimumLogLevelVerifier(minLevel), tableSchema);
        }

        public static DbLogger DbLogger(
            this ILoggingWizardCharms charms,
            string connectionString,
            ILogRecordVerifier recordVerifier,
            Action<LogTableSchemaConfig> schemaApply
            )
        {
            var args = new LogTableSchemaConfig();

            schemaApply(args);

            var schema = new LogTableSchema(args.TableName, args.InitScript, args.InserLogScript, args.Columns);

            return new DbLogger(connectionString)
            {
                TableSchema = schema,
                RecordVerifier = recordVerifier
            };
        }

        public static DbLogger DbLogger(
            this ILoggingWizardCharms charms,
            string connectionString,
            LogLevel minLevel,
            Action<LogTableSchemaConfig> schemaApply
            )
        {
            return DbLogger(charms, connectionString, new MinimumLogLevelVerifier(minLevel), schemaApply);
        }

        public static DbLogger DbLogger(
            this ILoggingWizardCharms charms,
            string connectionString,
            Action<LogTableSchemaConfig> schemaApply
            )
        {
            return DbLogger(charms, connectionString, new MinimumLogLevelVerifier(LogLevel.Debug), schemaApply);
        }

        public static DbLogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogTableSchema tableSchema
           )
        {
            return DbLogger(charms, connectionString, new MinimumLogLevelVerifier(LogLevel.Debug), tableSchema);
        }

        #endregion Db

        #region Debug

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

        public static DebugLogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel = LogLevel.Debug,
           IStringLogSerializer serializer = null
           )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(minLevel), serializer);
        }

        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return DebugLogger(charms, recordVerifier, serializer);
        }

        public static DebugLogger DebugLogger(
           this ILoggingWizardCharms charms,
           LogLevel minLevel,
           Action<StringLogSerializer> serializerApply
           )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(minLevel), serializerApply);
        }

        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply
            )
        {
            return DebugLogger(charms, new MinimumLogLevelVerifier(LogLevel.Debug), serializerApply);
        }

        #endregion Debug

        #region Empty

        public static EmptyLogger EmptyLogger(
            this ILoggingWizardCharms charms
            )
        {
            return new EmptyLogger();
        }

        #endregion Empty

        #region Lambda

        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            ILogRecordVerifier recordVerifier
            )
        {
            return new LambdaLogger(writeAction) { RecordVerifier = recordVerifier };
        }

        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(charms, writeAction, new MinimumLogLevelVerifier(minLevel));
        }

        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogRecord> writeAction
            )
        {
            return LambdaLogger(charms, writeAction, new MinimumLogLevelVerifier(LogLevel.Debug));
        }

        #endregion Lambda

        #region LoadFromJson

        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           string filePath,
           string sectionPath
           )
        {
            return new LoggerJsonLoader().Load(filePath, sectionPath);
        }

        public static ILogger LoadFromJson(
           this ILoggingWizardCharms charms,
           JToken loggingSection
           )
        {
            return new LoggerJsonLoader().Load(loggingSection);
        }

        #endregion LoadFromJson

        #region Stream

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            ILogRecordVerifier recordVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            return new StreamLogger(writer, autoFlush, leaveOpen)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier
            };
        }

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            return StreamLogger(charms, writer, new MinimumLogLevelVerifier(minLevel), serializer, encoding, autoFlush, leaveOpen);
        }

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            return StreamLogger(charms, writer, new MinimumLogLevelVerifier(LogLevel.Debug), serializer, encoding, autoFlush, leaveOpen);
        }

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            ILogRecordVerifier recordVerifier,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new StreamLogger(writer, autoFlush, leaveOpen)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = recordVerifier
            };
        }

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new StreamLogger(writer, autoFlush, leaveOpen)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(minLevel)
            };
        }

        public static StreamLogger StreamLogger(
            this ILoggingWizardCharms charms,
            Stream writer,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new StreamLogger(writer, autoFlush, leaveOpen)
            {
                Serializer = serializer,
                Encoding = encoding,
                RecordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug)
            };
        }

        #endregion Stream

        #region Background

        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            ILogger logger,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false
            )
        {
            return new BackgroundLogger(logger, flushPeriod, batchSize, leaveOpen);
        }

        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            ILogRecordVerifier recordVerifier,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false
            )
        {
            var logger = loggerFactory(charms);

            return BackgroundLogger(charms, logger, recordVerifier, flushPeriod, batchSize, leaveOpen);
        }

        public static BackgroundLogger BackgroundLogger(
           this ILoggingWizardCharms charms,
           ILogger logger,
           LogLevel minLevel = LogLevel.Debug,
           TimeSpan? flushPeriod = null,
           int? batchSize = null,
           bool leaveOpen = false
           )
        {
            return BackgroundLogger(charms, logger, new MinimumLogLevelVerifier(minLevel), flushPeriod, batchSize, leaveOpen);
        }

        public static BackgroundLogger BackgroundLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            LogLevel minLevel = LogLevel.Debug,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false
            )
        {
            var logger = loggerFactory(charms);

            return BackgroundLogger(charms, logger, new MinimumLogLevelVerifier(minLevel), flushPeriod, batchSize, leaveOpen);
        }

        #endregion Background

        #region ThreadSave

        //public static ThreadSafeLogger ThreadSafeLogger(
        //    this ILoggingWizardCharms charms,
        //    ILogger logger,
        //    ILogLevelVerifier recordVerifier,
        //    bool leaveOpen = false
        //    )
        //{
        //    return new ThreadSafeLogger(logger, leaveOpen);
        //}

        //public static ThreadSafeLogger ThreadSafeLogger(
        //    this ILoggingWizardCharms charms,
        //    Func<ILoggingWizardCharms, ILogger> loggerFactory,
        //    ILogLevelVerifier recordVerifier,
        //    bool leaveOpen = false
        //    )
        //{
        //    var logger = loggerFactory(charms);

        //    return ThreadSafeLogger(charms, logger, recordVerifier, leaveOpen);
        //}

        //public static ThreadSafeLogger ThreadSafeLogger(
        //   this ILoggingWizardCharms charms,
        //   ILogger logger,
        //   LogLevel minLevel = LogLevel.Debug,
        //    bool leaveOpen = false
        //   )
        //{
        //    return ThreadSafeLogger(charms, logger, new MinimumLogLevelVerifier(minLevel), leaveOpen);
        //}

        //public static ThreadSafeLogger ThreadSafeLogger(
        //    this ILoggingWizardCharms charms,
        //    Func<ILoggingWizardCharms, ILogger> loggerFactory,
        //    LogLevel minLevel = LogLevel.Debug,
        //    bool leaveOpen = false
        //    )
        //{
        //    var logger = loggerFactory(charms);

        //    return ThreadSafeLogger(charms, logger, new MinimumLogLevelVerifier(minLevel), leaveOpen);
        //}

        #endregion ThreadSave
    }
}