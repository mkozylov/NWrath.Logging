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
           ILogLevelVerifier levelVerifier,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           FileMode fileMode = FileMode.Append
           )
        {
            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                LevelVerifier = levelVerifier,
                FileMode = fileMode
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append
            )
        {
            return FileLogger(charms, filePath, new MinimumLogLevelVerifier(minLevel), serializer, encoding, fileMode);
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append
            )
        {
            return FileLogger(charms, filePath, new MinimumLogLevelVerifier(LogLevel.Debug), serializer, encoding, fileMode);
        }

        public static FileLogger FileLogger(
          this ILoggingWizardCharms charms,
          string filePath,
          ILogLevelVerifier levelVerifier,
          Action<StringLogSerializer> serializerApply,
          Encoding encoding = null,
          FileMode fileMode = FileMode.Append
          )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                LevelVerifier = levelVerifier,
                FileMode = fileMode
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            LogLevel minLevel,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                LevelVerifier = new MinimumLogLevelVerifier(minLevel),
                FileMode = fileMode
            };
        }

        public static FileLogger FileLogger(
            this ILoggingWizardCharms charms,
            string filePath,
            Action<StringLogSerializer> serializerApply,
            Encoding encoding = null,
            FileMode fileMode = FileMode.Append
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new FileLogger(filePath)
            {
                Serializer = serializer,
                Encoding = encoding,
                LevelVerifier = new MinimumLogLevelVerifier(LogLevel.Debug),
                FileMode = fileMode
            };
        }

        #endregion File

        #region Console

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            ILogLevelVerifier levelVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return new ConsoleLogger()
            {
                Serializer = serializer,
                LevelVerifier = levelVerifier
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
            ILogLevelVerifier levelVerifier,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return new ConsoleLogger
            {
                Serializer = serializer,
                LevelVerifier = levelVerifier
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

        #region PipeLogger

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
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            collection.AddRange(pipes);

            return PipeLogger(charms, logger, collection, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null
            )
            where TLogger : class, ILogger
        {
            return new PipeLogger<TLogger>(logger) { Pipes = pipes, Properties = properties };
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           TLogger logger,
           IPipe<PipeLoggerContext<TLogger>>[] pipes,
           Set properties = null
           )
           where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            collection.AddRange(pipes);

            return PipeLogger(charms, logger, collection, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null
            )
            where TLogger : class, ILogger
        {
            var collection = new PipeCollection<PipeLoggerContext<TLogger>>();

            pipesApply(collection);

            return PipeLogger(charms, logger, collection, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           Func<ILoggingWizardCharms, TLogger> loggerFactory,
           IPipe<PipeLoggerContext<TLogger>>[] pipes,
           Set properties = null
           )
           where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties);
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
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipes, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, TLogger> loggerFactory,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null
            )
            where TLogger : class, ILogger
        {
            return PipeLogger(charms, loggerFactory(charms), pipesApply, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            PipeCollection<PipeLoggerContext<TLogger>> pipes,
            Set properties = null
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            IPipe<PipeLoggerContext<TLogger>>[] pipes,
            Set properties = null
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties);
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
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] pipes
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipes, properties);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipesApply,
            Set properties = null
            )
            where TLogger : class, ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipesApply, properties);
        }

        #endregion PipeLogger

        #region RollingFile

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            ILogLevelVerifier levelVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            var logger = new RollingFileLogger(folderPath)
            {
                LevelVerifier = levelVerifier
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
           ILogLevelVerifier levelVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return RollingFileLogger(charms, folderPath, levelVerifier, serializer, encoding, pipes);
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
            ILogLevelVerifier levelVerifier,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            var logger = new RollingFileLogger(fileProvider)
            {
                LevelVerifier = levelVerifier
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
           ILogLevelVerifier levelVerifier,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return RollingFileLogger(charms, fileProvider, levelVerifier, serializer, encoding, pipes);
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

        #region CompositeLogger

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params ILogger[] loggers
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            LogLevel minLevel,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, new MinimumLogLevelVerifier(minLevel), loggerFactories);
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           ILogLevelVerifier levelVerifier,
           params ILogger[] loggers
           )
        {
            return new CompositeLogger(loggers) { LevelVerifier = levelVerifier };
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            ILogLevelVerifier levelVerifier,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .ToArray();

            return CompositeLogger(charms, levelVerifier, loggers);
        }

        public static CompositeLogger CompositeLogger(
           this ILoggingWizardCharms charms,
           params ILogger[] loggers
           )
        {
            return CompositeLogger(charms, LogLevel.Debug, loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            return CompositeLogger(charms, LogLevel.Debug, loggerFactories);
        }

        #endregion CompositeLogger

        #region DbLogger

        public static DbLogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           ILogLevelVerifier levelVerifier,
           ILogTableSchema tableSchema = null
           )
        {
            return new DbLogger(connectionString)
            {
                TableSchema = tableSchema,
                LevelVerifier = levelVerifier
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
            ILogLevelVerifier levelVerifier,
            Action<LogTableSchemaConfig> schemaApply
            )
        {
            var args = new LogTableSchemaConfig();

            schemaApply(args);

            var schema = new LogTableSchema(args.TableName, args.InitScript, args.InserLogScript, args.Columns);

            return new DbLogger(connectionString)
            {
                TableSchema = schema,
                LevelVerifier = levelVerifier
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

        #endregion DbLogger

        #region Debug

        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
            ILogLevelVerifier levelVerifier,
            IStringLogSerializer serializer = null
            )
        {
            return new DebugLogger
            {
                Serializer = serializer,
                LevelVerifier = levelVerifier
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
            ILogLevelVerifier levelVerifier,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return DebugLogger(charms, levelVerifier, serializer);
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
            Action<LogMessage> writeAction,
            ILogLevelVerifier levelVerifier
            )
        {
            return new LambdaLogger(writeAction) { LevelVerifier = levelVerifier };
        }

        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogMessage> writeAction,
            LogLevel minLevel
            )
        {
            return LambdaLogger(charms, writeAction, new MinimumLogLevelVerifier(minLevel));
        }

        public static LambdaLogger LambdaLogger(
            this ILoggingWizardCharms charms,
            Action<LogMessage> writeAction
            )
        {
            return LambdaLogger(charms, writeAction, new MinimumLogLevelVerifier(LogLevel.Debug));
        }

        #endregion Lambda

        #region ThreadSave

        public static ThreadSafeLogger ThreadSafeLogger(
            this ILoggingWizardCharms charms,
            ILogger logger,
            ILogLevelVerifier levelVerifier
            )
        {
            return new ThreadSafeLogger(logger);
        }

        public static ThreadSafeLogger ThreadSafeLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            ILogLevelVerifier levelVerifier
            )
        {
            var logger = loggerFactory(charms);

            return ThreadSafeLogger(charms, logger, levelVerifier);
        }

        public static ThreadSafeLogger ThreadSafeLogger(
           this ILoggingWizardCharms charms,
           ILogger logger,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return ThreadSafeLogger(charms, logger, new MinimumLogLevelVerifier(minLevel));
        }

        public static ThreadSafeLogger ThreadSafeLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            var logger = loggerFactory(charms);

            return ThreadSafeLogger(charms, logger, new MinimumLogLevelVerifier(minLevel));
        }

        #endregion ThreadSave

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
    }
}