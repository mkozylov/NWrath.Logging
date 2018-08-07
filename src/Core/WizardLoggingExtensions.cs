using NWrath.Synergy.Reflection.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Pipeline;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NWrath.Logging
{
    public static class WizardLoggingExtensions
    {
        #region File

        public static FileLogger FileLogger(
           this ILoggingWizardCharms charms,
           string filePath,
           IStringLogSerializer serializer = null,
           Encoding encoding = null,
           bool append = false
           )
        {
            return new FileLogger(filePath, serializer, encoding, append);
        }

        #endregion File

        #region Console

        public static ConsoleLogger ConsoleLogger(
          this ILoggingWizardCharms charms,
          IStringLogSerializer serializer
          )
        {
            return new ConsoleLogger(serializer);
        }

        public static ConsoleLogger ConsoleLogger(
           this ILoggingWizardCharms charms,
           IConsoleLogSerializer serializer = null
           )
        {
            return new ConsoleLogger(serializer ?? new ConsoleLogSerializer());
        }

        public static ConsoleLogger ConsoleLogger(
            this ILoggingWizardCharms charms,
            Action<ConsoleLogSerializer> serializerApply
            )
        {
            var serializer = new ConsoleLogSerializer();

            serializerApply(serializer);

            return new ConsoleLogger(serializer);
        }

        #endregion Console

        #region PipeLogger

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipelineApply = null
            )
            where TLogger : ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), pipelineApply);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            Action<PipeCollection<PipeLoggerContext<TLogger>>> pipelineApply = null
            )
            where TLogger : ILogger
        {
            var pipes = default(PipeCollection<PipeLoggerContext<TLogger>>);

            if (pipelineApply != null)
            {
                pipes = new PipeCollection<PipeLoggerContext<TLogger>>();
                pipelineApply(pipes);
            }

            return new PipeLogger<TLogger>(logger, pipes);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           Func<ILoggingWizardCharms, TLogger> loggerFactory,
           Action<PipeCollection<PipeLoggerContext<TLogger>>> pipelineApply = null
           )
           where TLogger : ILogger
        {
            var logger = loggerFactory(charms);

            return PipeLogger(charms, logger, pipelineApply);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            params IPipe<PipeLoggerContext<TLogger>>[] additionalPipes
            )
            where TLogger : ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), additionalPipes);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            params IPipe<PipeLoggerContext<TLogger>>[] additionalPipes
            )
            where TLogger : ILogger
        {
            var l = new PipeLogger<TLogger>(logger);

            l.Pipes.AddRange(additionalPipes);

            return l;
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] additionalPipes
            )
            where TLogger : ILogger, new()
        {
            return PipeLogger(charms, new TLogger(), additionalPipes);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
            this ILoggingWizardCharms charms,
            TLogger logger,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] additionalPipes
            )
            where TLogger : ILogger
        {
            var l = new PipeLogger<TLogger>(logger);

            l.Pipes.AddRange(additionalPipes);

            return l;
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           Func<ILoggingWizardCharms, TLogger> loggerFactory,
           params IPipe<PipeLoggerContext<TLogger>>[] additionalPipes
           )
           where TLogger : ILogger
        {
            var logger = loggerFactory(charms);

            return PipeLogger(charms, logger, additionalPipes);
        }

        public static PipeLogger<TLogger> PipeLogger<TLogger>(
           this ILoggingWizardCharms charms,
           Func<ILoggingWizardCharms, TLogger> loggerFactory,
           params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] additionalPipes
           )
           where TLogger : ILogger
        {
            var logger = loggerFactory(charms);

            return PipeLogger(charms, logger, additionalPipes);
        }

        #endregion PipeLogger

        #region RollingFile

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return new RollingFileLogger(folderPath, serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
           this ILoggingWizardCharms charms,
           string folderPath,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new RollingFileLogger(folderPath, serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
            this ILoggingWizardCharms charms,
            IRollingFileProvider fileNameProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            return new RollingFileLogger(fileNameProvider, serializer, encoding, pipes);
        }

        public static RollingFileLogger RollingFileLogger(
           this ILoggingWizardCharms charms,
           IRollingFileProvider fileNameProvider,
           Action<StringLogSerializer> serializerApply,
           Encoding encoding = null,
           PipeCollection<RollingFileContext> pipes = null
           )
        {
            var serializer = new StringLogSerializer();

            serializerApply?.Invoke(serializer);

            return new RollingFileLogger(fileNameProvider, serializer, encoding, pipes);
        }

        #endregion RollingFile

        #region CompositeLogger

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params ILogger[] loggers
            )
        {
            return new CompositeLogger(loggers);
        }

        public static CompositeLogger CompositeLogger(
            this ILoggingWizardCharms charms,
            params Func<ILoggingWizardCharms, ILogger>[] loggerFactories
            )
        {
            var loggers = loggerFactories.Select(f => f(charms))
                                         .ToArray();

            return new CompositeLogger(loggers);
        }

        #endregion CompositeLogger

        #region DbLogger

        public static DbLogger DbLogger(
           this ILoggingWizardCharms charms,
           string connectionString,
           LogTableSchema logTableSchema = null
           )
        {
            return new DbLogger(connectionString, logTableSchema);
        }

        public static DbLogger DbLogger(
            this ILoggingWizardCharms charms,
            string connectionString,
            Action<LogTableSchema> schemaApply
            )
        {
            var schema = new LogTableSchema();

            schemaApply(schema);

            return new DbLogger(connectionString, schema);
        }

        #endregion DbLogger

        #region Debug

        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
            IStringLogSerializer serializer = null
            )
        {
            return new DebugLogger(serializer);
        }

        public static DebugLogger DebugLogger(
            this ILoggingWizardCharms charms,
            Action<StringLogSerializer> serializerApply
            )
        {
            var serializer = new StringLogSerializer();

            serializerApply(serializer);

            return new DebugLogger(serializer);
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
           Action<LogMessage> writeAction
           )
        {
            return new LambdaLogger(writeAction);
        }

        #endregion Lambda

        #region ThreadSave

        public static ThreadSaveLogger ThreadSaveLogger(
            this ILoggingWizardCharms charms,
            ILogger logger
            )
        {
            return new ThreadSaveLogger(logger);
        }

        public static ThreadSaveLogger ThreadSaveLogger(
            this ILoggingWizardCharms charms,
            Func<ILoggingWizardCharms, ILogger> loggerFactory
            )
        {
            var logger = loggerFactory(charms);

            return new ThreadSaveLogger(logger);
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