using Moq;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Pipeline;
using System;
using System.IO;
using System.Text;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class WizardLoggingExtensionsTests
    {
        [Test]
        public void WizardLoggingExtensions_FileLogger()
        {
            #region Arrange

            var path = Path.Combine(Environment.CurrentDirectory, "log.txt");
            var defaultMinLevel = LogLevel.Debug;
            var defaultEncoding = Encoding.UTF8;
            var defaultFileMode = FileMode.Append;
            var defaultOutputTemplate = StringLogSerializer.DefaultOutputTemplate;
            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new StringLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<StringLogSerializer>(s => { s.OutputTemplate = outputTemplate; });
            var encoding = Encoding.ASCII;
            var fileMode = FileMode.Truncate;

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.FileLogger(path);
            var logger2 = LoggingWizard.Spell.FileLogger(path, minLevel);
            var logger3 = LoggingWizard.Spell.FileLogger(path, levelVerifier);
            var logger4 = LoggingWizard.Spell.FileLogger(path, minLevel, serializer);
            var logger5 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializer);
            var logger6 = LoggingWizard.Spell.FileLogger(path, minLevel, serializerApply);
            var logger7 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializerApply);
            var logger8 = LoggingWizard.Spell.FileLogger(path, minLevel, serializer, encoding);
            var logger9 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializer, encoding);
            var logger10 = LoggingWizard.Spell.FileLogger(path, minLevel, serializerApply, encoding);
            var logger11 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializerApply, encoding);
            var logger12 = LoggingWizard.Spell.FileLogger(path, minLevel, serializer, encoding, fileMode);
            var logger13 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializer, encoding, fileMode);
            var logger14 = LoggingWizard.Spell.FileLogger(path, minLevel, serializerApply, encoding, fileMode);
            var logger15 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializerApply, encoding, fileMode);
            var logger16 = LoggingWizard.Spell.FileLogger(path, serializer);
            var logger17 = LoggingWizard.Spell.FileLogger(path, serializerApply);
            var logger18 = LoggingWizard.Spell.FileLogger(path, encoding: encoding);
            var logger19 = LoggingWizard.Spell.FileLogger(path, fileMode: fileMode);

            #endregion Act

            #region Assert

            Assert.AreEqual(defaultMinLevel, logger1.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(defaultOutputTemplate, logger1.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger1.Encoding);
            Assert.AreEqual(defaultFileMode, logger1.FileMode);
            Assert.AreEqual(path, logger1.FilePath);

            Assert.AreEqual(minLevel, logger2.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(defaultOutputTemplate, logger2.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger2.Encoding);
            Assert.AreEqual(defaultFileMode, logger2.FileMode);
            Assert.AreEqual(path, logger2.FilePath);

            Assert.AreEqual(defaultOutputTemplate, logger3.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger3.Encoding);
            Assert.AreEqual(defaultFileMode, logger3.FileMode);
            Assert.AreEqual(levelVerifier, logger3.LevelVerifier);
            Assert.AreEqual(path, logger3.FilePath);

            Assert.AreEqual(path, logger4.FilePath);
            Assert.AreEqual(minLevel, logger4.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(serializer, logger4.Serializer);

            Assert.AreEqual(defaultEncoding, logger5.Encoding);
            Assert.AreEqual(defaultFileMode, logger5.FileMode);
            Assert.AreEqual(levelVerifier, logger5.LevelVerifier);
            Assert.AreEqual(serializer, logger5.Serializer);
            Assert.AreEqual(path, logger5.FilePath);

            Assert.AreEqual(minLevel, logger6.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(outputTemplate, logger6.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger6.Encoding);
            Assert.AreEqual(defaultFileMode, logger6.FileMode);
            Assert.AreEqual(path, logger6.FilePath);

            Assert.AreEqual(outputTemplate, logger7.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger7.Encoding);
            Assert.AreEqual(defaultFileMode, logger7.FileMode);
            Assert.AreEqual(levelVerifier, logger7.LevelVerifier);
            Assert.AreEqual(path, logger7.FilePath);

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_DbLogger()
        {
            #region Arrange

            var connectionString = "cs";
            var tableName = "Log";
            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var tableSchema = new LogTableSchema(tableName);
            var tableSchemaApply = new Action<LogTableSchemaConfig>(s => s.TableName = tableName);
            var defTableSchema = new LogTableSchema();

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.DbLogger(connectionString);
            var logger2 = LoggingWizard.Spell.DbLogger(connectionString, minLevel);
            var logger3 = LoggingWizard.Spell.DbLogger(connectionString, levelVerifier);
            var logger4 = LoggingWizard.Spell.DbLogger(connectionString, minLevel, tableSchema);
            var logger5 = LoggingWizard.Spell.DbLogger(connectionString, levelVerifier, tableSchema);
            var logger6 = LoggingWizard.Spell.DbLogger(connectionString, minLevel, tableSchemaApply);
            var logger7 = LoggingWizard.Spell.DbLogger(connectionString, levelVerifier, tableSchemaApply);
            var logger9 = LoggingWizard.Spell.DbLogger(connectionString, tableSchema);
            var logger10 = LoggingWizard.Spell.DbLogger(connectionString, tableSchemaApply);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_ConsoleLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new StringLogSerializer { OutputTemplate = outputTemplate };
            var consoleSerializer = new ConsoleLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<ConsoleLogSerializer>(s => { s.OutputTemplate = outputTemplate; });

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.ConsoleLogger();
            var logger2 = LoggingWizard.Spell.ConsoleLogger(minLevel);
            var logger3 = LoggingWizard.Spell.ConsoleLogger(levelVerifier);
            var logger4 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializer);
            var logger5 = LoggingWizard.Spell.ConsoleLogger(levelVerifier, serializer);
            var logger6 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializerApply);
            var logger7 = LoggingWizard.Spell.ConsoleLogger(levelVerifier, serializerApply);
            var logger9 = LoggingWizard.Spell.ConsoleLogger(serializer);
            var logger10 = LoggingWizard.Spell.ConsoleLogger(serializerApply);
            var logger11 = LoggingWizard.Spell.ConsoleLogger(consoleSerializer);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_PipeLogger()
        {
            #region Arrange

            var stubPipe = LambdaPipe<PipeLoggerContext<EmptyLogger>>.Stub;
            var stubPipeDelegate = new Action<PipeLoggerContext<EmptyLogger>, Action<PipeLoggerContext<EmptyLogger>>>(
                (c, n) => { stubPipe.Perform(c); n(c); }
                );

            var pipeCollection = new PipeCollection<PipeLoggerContext<EmptyLogger>>().Apply(c => c.Add(stubPipe));
            var pipeArray = new IPipe<PipeLoggerContext<EmptyLogger>>[] { stubPipe };
            var pipeDelegates = new Action<PipeLoggerContext<EmptyLogger>, Action<PipeLoggerContext<EmptyLogger>>>[] { stubPipeDelegate };
            var pipesApply = new Action<PipeCollection<PipeLoggerContext<EmptyLogger>>>(c => c.Add(stubPipe));

            var properties = new Set();

            var pipedLogger = new EmptyLogger();

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.PipeLogger(pipedLogger);
            var logger2 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeCollection);
            var logger3 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeArray);
            var logger4 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeDelegates);
            var logger5 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipesApply);
            var logger6 = LoggingWizard.Spell.PipeLogger(pipedLogger, stubPipeDelegate);
            var logger7 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeCollection, properties);
            var logger8 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeArray, properties);
            var logger9 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipeDelegates, properties);
            var logger10 = LoggingWizard.Spell.PipeLogger(pipedLogger, pipesApply, properties);
            var logger11 = LoggingWizard.Spell.PipeLogger(pipedLogger, properties, stubPipeDelegate);

            var logger12 = LoggingWizard.Spell.PipeLogger<EmptyLogger>();
            var logger13 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeCollection);
            var logger14 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeArray);
            var logger15 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeDelegates);
            var logger16 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipesApply);
            var logger17 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(stubPipeDelegate);
            var logger18 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeCollection, properties);
            var logger19 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeArray, properties);
            var logger20 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipeDelegates, properties);
            var logger21 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(pipesApply, properties);
            var logger22 = LoggingWizard.Spell.PipeLogger<EmptyLogger>(properties, stubPipeDelegate);

            var logger23 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger());
            var logger24 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeCollection);
            var logger25 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeArray);
            var logger26 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeDelegates);
            var logger27 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipesApply);
            var logger28 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), stubPipeDelegate);
            var logger29 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeCollection, properties);
            var logger30 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeArray, properties);
            var logger31 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipeDelegates, properties);
            var logger32 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), pipesApply, properties);
            var logger33 = LoggingWizard.Spell.PipeLogger(s => s.EmptyLogger(), properties, stubPipeDelegate);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_DebugLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new ConsoleLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<ConsoleLogSerializer>(s => { s.OutputTemplate = outputTemplate; });

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.ConsoleLogger();
            var logger2 = LoggingWizard.Spell.ConsoleLogger(minLevel);
            var logger3 = LoggingWizard.Spell.ConsoleLogger(levelVerifier);
            var logger4 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializer);
            var logger5 = LoggingWizard.Spell.ConsoleLogger(levelVerifier, serializer);
            var logger6 = LoggingWizard.Spell.ConsoleLogger(minLevel, serializerApply);
            var logger7 = LoggingWizard.Spell.ConsoleLogger(levelVerifier, serializerApply);
            var logger8 = LoggingWizard.Spell.ConsoleLogger(serializer);
            var logger9 = LoggingWizard.Spell.ConsoleLogger(serializerApply);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_EmptyLogger()
        {
            #region Arrange

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.EmptyLogger();

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_CompositeLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var loggers = new ILogger[] { new EmptyLogger(), new EmptyLogger() };
            var loggerFactories = new Func<ILoggingWizardCharms, ILogger>[] {
                s => s.EmptyLogger(),
                s => s.EmptyLogger()
            };

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.CompositeLogger(loggers);
            var logger2 = LoggingWizard.Spell.CompositeLogger(loggerFactories);
            var logger3 = LoggingWizard.Spell.CompositeLogger(minLevel, loggers);
            var logger4 = LoggingWizard.Spell.CompositeLogger(minLevel, loggerFactories);
            var logger5 = LoggingWizard.Spell.CompositeLogger(levelVerifier, loggers);
            var logger6 = LoggingWizard.Spell.CompositeLogger(levelVerifier, loggerFactories);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_LambdaLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var writeAction = new Action<LogMessage>(m => { });

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.LambdaLogger(writeAction);
            var logger2 = LoggingWizard.Spell.LambdaLogger(writeAction, minLevel);
            var logger3 = LoggingWizard.Spell.LambdaLogger(writeAction, levelVerifier);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_ThreadSafeLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var safeLogger = new Mock<ILogger>().Object;
            var safeLoggerFactory = new Func<ILoggingWizardCharms, ILogger>(s => new Mock<ILogger>().Object);

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.ThreadSafeLogger(safeLogger);
            var logger2 = LoggingWizard.Spell.ThreadSafeLogger(safeLogger, minLevel);
            var logger3 = LoggingWizard.Spell.ThreadSafeLogger(safeLogger, levelVerifier);
            var logger4 = LoggingWizard.Spell.ThreadSafeLogger(safeLoggerFactory, minLevel);
            var logger5 = LoggingWizard.Spell.ThreadSafeLogger(safeLoggerFactory, levelVerifier);

            #endregion Act

            #region Assert

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_RollingFileLogger()
        {
            #region Arrange

            var defaultMinLevel = LogLevel.Debug;
            var defaultEncoding = Encoding.UTF8;
            var defaultOutputTemplate = StringLogSerializer.DefaultOutputTemplate;
            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new StringLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<StringLogSerializer>(s => { s.OutputTemplate = outputTemplate; });
            var encoding = Encoding.ASCII;
            var folder = "folder";
            var fileNameProvider = new Mock<IRollingFileProvider>().Object;
            var stubPipe = LambdaPipe<RollingFileContext>.Stub;
            var stubPipeDelegate = new Action<RollingFileContext, Action<RollingFileContext>>(
                (c, n) => { stubPipe.Perform(c); n(c); }
                );
            var pipeCollection = new PipeCollection<RollingFileContext>().Apply(c => c.Add(stubPipe));

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.RollingFileLogger(folder);
            var logger2 = LoggingWizard.Spell.RollingFileLogger(folder, serializer, encoding, pipeCollection);
            var logger3 = LoggingWizard.Spell.RollingFileLogger(folder, serializerApply, encoding, pipeCollection);
            var logger4 = LoggingWizard.Spell.RollingFileLogger(folder, minLevel, serializer, encoding, pipeCollection);
            var logger5 = LoggingWizard.Spell.RollingFileLogger(folder, levelVerifier, serializer, encoding, pipeCollection);
            var logger6 = LoggingWizard.Spell.RollingFileLogger(folder, minLevel, serializerApply, encoding, pipeCollection);
            var logger7 = LoggingWizard.Spell.RollingFileLogger(folder, levelVerifier, serializerApply, encoding, pipeCollection);
            var logger8 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider);
            var logger9 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, serializer, encoding, pipeCollection);
            var logger10 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, serializerApply, encoding, pipeCollection);
            var logger11 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, minLevel, serializer, encoding, pipeCollection);
            var logger12 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, levelVerifier, serializer, encoding, pipeCollection);
            var logger13 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, minLevel, serializerApply, encoding, pipeCollection);
            var logger14 = LoggingWizard.Spell.RollingFileLogger(fileNameProvider, levelVerifier, serializerApply, encoding, pipeCollection);

            #endregion Act

            #region Assert

            Assert.IsNotEmpty(logger1.Pipes);
            Assert.AreEqual(defaultMinLevel, logger1.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(defaultOutputTemplate, logger1.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger1.Encoding);
            Assert.AreEqual(folder, logger1.FileProvider.FolderPath);

            Assert.AreEqual(pipeCollection, logger2.Pipes);
            Assert.AreEqual(serializer, logger2.Serializer);
            Assert.AreEqual(encoding, logger2.Encoding);
            Assert.AreEqual(defaultMinLevel, logger2.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(folder, logger2.FileProvider.FolderPath);

            #endregion Assert
        }
    }
}