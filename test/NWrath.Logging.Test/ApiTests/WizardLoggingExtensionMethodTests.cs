using NUnit.Framework;
using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class WizardLoggingExtensionMethodTests
    {
        [Test]
        public void WizardLoggingExtensionMethods_FileLogger()
        {
            var file = Path.GetTempFileName();
            var serializer = new StringLogSerializer();
            var serializerApply = new Action<StringLogSerializer>(s => { });
            var enc = Encoding.UTF8;
            var append = true;
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

            var l1 = LoggingWizard.Spell.FileLogger(file, serializer, enc, append);
            var l2 = LoggingWizard.Spell.FileLogger(file, serializerApply, enc, append);
            var l3 = LoggingWizard.Spell.FileLogger(file, recordVerifier, serializer, enc, append);
            var l4 = LoggingWizard.Spell.FileLogger(file, recordVerifier, serializerApply, enc, append);
            var l5 = LoggingWizard.Spell.FileLogger(file, LogLevel.Debug, serializer, enc, append);
            var l6 = LoggingWizard.Spell.FileLogger(file, LogLevel.Debug, serializerApply, enc, append);
        }

        [Test]
        public void WizardLoggingExtensionMethods_SqlLogger()
        {
            var connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True;Connection Timeout=3";
            var schema = new SqlLogSchema(connectionString);
            var schemaApply = new Action<SqlLogSchemaConfig>(s => {
                s.ConnectionString = connectionString;
            });
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);

            var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var l1 = LoggingWizard.Spell.SqlLogger(schema);
            var l2 = LoggingWizard.Spell.SqlLogger(schemaApply);
            var l3 = LoggingWizard.Spell.SqlLogger(recordVerifier, schemaApply);
            var l4 = LoggingWizard.Spell.SqlLogger(LogLevel.Debug, schemaApply);
            var l5 = LoggingWizard.Spell.SqlLogger(schema, recordVerifier);
            var l6 = LoggingWizard.Spell.SqlLogger(schema, LogLevel.Debug);
            var l7 = LoggingWizard.Spell.SqlLogger(connectionString, recordVerifier);
            var l8 = LoggingWizard.Spell.SqlLogger(connectionString, LogLevel.Debug);
        }

        [Test]
        public void WizardLoggingExtensionMethods_ConsoleLogger()
        {
            var serializer = new StringLogSerializer();
            var serializerApply = new Action<ConsoleLogSerializer>(s => { });
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;

            var l1 = LoggingWizard.Spell.ConsoleLogger(serializer);
            var l2 = LoggingWizard.Spell.ConsoleLogger(serializerApply);
            var l3 = LoggingWizard.Spell.ConsoleLogger(recordVerifier, serializer);
            var l4 = LoggingWizard.Spell.ConsoleLogger(recordVerifier, serializerApply);
            var l5 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializer);
            var l6 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializerApply);
        }

        [Test]
        public void WizardLoggingExtensionMethods_DebugLogger()
        {
            var serializer = new StringLogSerializer();
            var serializerApply = new Action<StringLogSerializer>(s => { });
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;

            var l1 = LoggingWizard.Spell.DebugLogger(serializer);
            var l2 = LoggingWizard.Spell.DebugLogger(serializerApply);
            var l3 = LoggingWizard.Spell.DebugLogger(recordVerifier, serializer);
            var l4 = LoggingWizard.Spell.DebugLogger(recordVerifier, serializerApply);
            var l5 = LoggingWizard.Spell.DebugLogger(minLevel, serializer);
            var l6 = LoggingWizard.Spell.DebugLogger(minLevel, serializerApply);
        }

        [Test]
        public void WizardLoggingExtensionMethods_EmptyLogger()
        {
            var l1 = LoggingWizard.Spell.EmptyLogger();
        }

        [Test]
        public void WizardLoggingExtensionMethods_CompositeLogger()
        {
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;
            var leaveOpen = false;
            var loggers = new ILogger[]{ new EmptyLogger() };
            var loggerFactories = new Func<ILoggingWizardCharms, ILogger>[] {
                new Func<ILoggingWizardCharms, ILogger>(s => new EmptyLogger())
            };

            var l1 = LoggingWizard.Spell.CompositeLogger(loggers);
            var l2 = LoggingWizard.Spell.CompositeLogger(loggerFactories);
            var l3 = LoggingWizard.Spell.CompositeLogger(leaveOpen, loggers);
            var l4 = LoggingWizard.Spell.CompositeLogger(leaveOpen, loggerFactories);
            var l5 = LoggingWizard.Spell.CompositeLogger(recordVerifier, loggers);
            var l6 = LoggingWizard.Spell.CompositeLogger(recordVerifier, loggerFactories);
            var l7 = LoggingWizard.Spell.CompositeLogger(minLevel, loggers);
            var l8 = LoggingWizard.Spell.CompositeLogger(minLevel, loggerFactories);
            var l9 = LoggingWizard.Spell.CompositeLogger(recordVerifier, leaveOpen, loggers);
            var l10 = LoggingWizard.Spell.CompositeLogger(recordVerifier, leaveOpen, loggerFactories);
            var l11 = LoggingWizard.Spell.CompositeLogger(minLevel, leaveOpen, loggers);
            var l12 = LoggingWizard.Spell.CompositeLogger(minLevel, leaveOpen, loggerFactories);
        }

        [Test]
        public void WizardLoggingExtensionMethods_LambdaLogger()
        {
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;
            var writeAction = new Action<LogRecord>(r => { });
            var batchAction = new Action<LogRecord[]>(b => { });

            var l1 = LoggingWizard.Spell.LambdaLogger(batchAction);
            var l2 = LoggingWizard.Spell.LambdaLogger(writeAction);
            var l3 = LoggingWizard.Spell.LambdaLogger(batchAction, recordVerifier);
            var l4 = LoggingWizard.Spell.LambdaLogger(batchAction, minLevel);
            var l5 = LoggingWizard.Spell.LambdaLogger(writeAction, recordVerifier);
            var l6 = LoggingWizard.Spell.LambdaLogger(writeAction, minLevel);
            var l7 = LoggingWizard.Spell.LambdaLogger(writeAction, batchAction);
            var l8 = LoggingWizard.Spell.LambdaLogger(writeAction, batchAction, recordVerifier);
            var l9 = LoggingWizard.Spell.LambdaLogger(writeAction, batchAction, minLevel);
        }

        [Test]
        public void WizardLoggingExtensionMethods_RollingFileLogger()
        {
            var currentFolderPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var folderPath = Path.Combine(currentFolderPath, "TestLogs");
            var fileProvider = new RollingFileProvider(folderPath);
            var serializer = new StringLogSerializer();
            var serializerApply = new Action<StringLogSerializer>(s => { });
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;
            var enc = Encoding.UTF8;
            var pipes = new PipeCollection<RollingFileContext>();

            var l1 = LoggingWizard.Spell.RollingFileLogger(fileProvider, serializer, enc, pipes);
            var l2 = LoggingWizard.Spell.RollingFileLogger(fileProvider, serializerApply, enc, pipes);
            var l3 = LoggingWizard.Spell.RollingFileLogger(folderPath, serializer, enc, pipes);
            var l4 = LoggingWizard.Spell.RollingFileLogger(folderPath, serializerApply, enc, pipes);
            var l5 = LoggingWizard.Spell.RollingFileLogger(fileProvider, recordVerifier, serializer, enc, pipes);
            var l6 = LoggingWizard.Spell.RollingFileLogger(fileProvider, recordVerifier, serializerApply, enc, pipes);
            var l7 = LoggingWizard.Spell.RollingFileLogger(fileProvider, minLevel, serializer, enc, pipes);
            var l8 = LoggingWizard.Spell.RollingFileLogger(fileProvider, minLevel, serializerApply, enc, pipes);
            var l9 = LoggingWizard.Spell.RollingFileLogger(folderPath, recordVerifier, serializer, enc, pipes);
            var l10 = LoggingWizard.Spell.RollingFileLogger(folderPath, recordVerifier, serializerApply, enc, pipes);
            var l11 = LoggingWizard.Spell.RollingFileLogger(folderPath, minLevel, serializer, enc, pipes);
            var l12 = LoggingWizard.Spell.RollingFileLogger(folderPath, minLevel, serializerApply, enc, pipes);
        }

        [Test]
        public void WizardLoggingExtensionMethods_BackgroundLogger()
        {
            var recordVerifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var minLevel = LogLevel.Debug;
            var batchSize = 100;
            var flushPeriod = TimeSpan.FromSeconds(1);
            var leaveOpen = false;
            var batchAction = new Action<LogRecord[]>(b => { });
            var baseLogger = new EmptyLogger();
            var reserveLogger = new EmptyLogger();
            var loggerSetFactory = new Func<ILoggingWizardCharms, ILogger[]>(s => new ILogger[] { new EmptyLogger() });
            var loggerFactory = new Func<ILoggingWizardCharms, ILogger>(s => new EmptyLogger());

            var l1 = LoggingWizard.Spell.BackgroundLogger(baseLogger, recordVerifier, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l2 = LoggingWizard.Spell.BackgroundLogger(baseLogger, minLevel, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l3 = LoggingWizard.Spell.BackgroundLogger(batchAction, recordVerifier, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l4 = LoggingWizard.Spell.BackgroundLogger(batchAction, minLevel, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l5 = LoggingWizard.Spell.BackgroundLogger(loggerSetFactory, recordVerifier, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l6 = LoggingWizard.Spell.BackgroundLogger(loggerSetFactory, minLevel, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l7 = LoggingWizard.Spell.BackgroundLogger(loggerFactory, recordVerifier, flushPeriod, batchSize, leaveOpen, reserveLogger);
            var l8 = LoggingWizard.Spell.BackgroundLogger(loggerFactory, minLevel, flushPeriod, batchSize, leaveOpen, reserveLogger);
        }
    }
}
