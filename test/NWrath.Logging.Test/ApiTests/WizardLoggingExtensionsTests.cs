﻿using Moq;
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
            var defaultEncoding = new UTF8Encoding(false);
            var defaultFileMode = FileMode.Append;
            var defaultOutputTemplate = StringLogSerializer.DefaultOutputTemplate;
            var defaultAutoFlush = true;
            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new StringLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<StringLogSerializer>(s => { s.OutputTemplate = outputTemplate; });
            var encoding = Encoding.ASCII;
            var fileMode = FileMode.Truncate;
            var autoFlush = false;

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.FileLogger(path, serializer, encoding, fileMode, autoFlush);
            var logger2 = LoggingWizard.Spell.FileLogger(path, serializerApply, encoding, fileMode, autoFlush);
            var logger3 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializer, encoding, fileMode, autoFlush);
            var logger4 = LoggingWizard.Spell.FileLogger(path, levelVerifier, serializerApply, encoding, fileMode, autoFlush);
            var logger5 = LoggingWizard.Spell.FileLogger(path, minLevel, serializer, encoding, fileMode, autoFlush);
            var logger6 = LoggingWizard.Spell.FileLogger(path, minLevel, serializerApply, encoding, fileMode, autoFlush);
            var logger7 = LoggingWizard.Spell.FileLogger(path);

            #endregion Act

            #region Assert

            //1
            Assert.AreEqual(defaultMinLevel, logger1.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(serializer, logger1.Serializer);
            Assert.AreEqual(encoding, logger1.Encoding);
            Assert.AreEqual(fileMode, logger1.FileMode);
            Assert.AreEqual(path, logger1.FilePath);
            Assert.AreEqual(autoFlush, logger1.AutoFlush);

            //2
            Assert.AreEqual(defaultMinLevel, logger2.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(outputTemplate, logger2.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(encoding, logger2.Encoding);
            Assert.AreEqual(fileMode, logger2.FileMode);
            Assert.AreEqual(path, logger2.FilePath);
            Assert.AreEqual(autoFlush, logger2.AutoFlush);

            //3
            Assert.AreEqual(outputTemplate, logger3.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(encoding, logger3.Encoding);
            Assert.AreEqual(fileMode, logger3.FileMode);
            Assert.AreEqual(levelVerifier, logger3.RecordVerifier);
            Assert.AreEqual(path, logger3.FilePath);
            Assert.AreEqual(autoFlush, logger3.AutoFlush);

            //4
            Assert.AreEqual(levelVerifier, logger4.RecordVerifier);
            Assert.AreEqual(outputTemplate, logger4.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(encoding, logger4.Encoding);
            Assert.AreEqual(fileMode, logger4.FileMode);
            Assert.AreEqual(path, logger4.FilePath);
            Assert.AreEqual(autoFlush, logger4.AutoFlush);

            //5
            Assert.AreEqual(encoding, logger5.Encoding);
            Assert.AreEqual(fileMode, logger5.FileMode);
            Assert.AreEqual(minLevel, logger5.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(serializer, logger5.Serializer);
            Assert.AreEqual(path, logger5.FilePath);
            Assert.AreEqual(autoFlush, logger5.AutoFlush);

            //6
            Assert.AreEqual(encoding, logger6.Encoding);
            Assert.AreEqual(fileMode, logger6.FileMode);
            Assert.AreEqual(minLevel, logger6.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(outputTemplate, logger6.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(path, logger6.FilePath);
            Assert.AreEqual(autoFlush, logger6.AutoFlush);

            //7
            Assert.AreEqual(path, logger7.FilePath);
            Assert.AreEqual(defaultEncoding, logger7.Encoding);
            Assert.AreEqual(defaultFileMode, logger7.FileMode);
            Assert.AreEqual(defaultMinLevel, logger7.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(defaultOutputTemplate, logger7.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultAutoFlush, logger7.AutoFlush);

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
            var tableSchema = new SqlLogTableSchema(tableName);
            var tableSchemaApply = new Action<LogTableSchemaConfig>(s => s.TableName = tableName);
            var defTableSchema = new SqlLogTableSchema();

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
            var writeAction = new Action<LogRecord>(m => { });

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
        public void WizardLoggingExtensions_RollingFileLogger()
        {
            #region Arrange

            var defaultMinLevel = LogLevel.Debug;
            var defaultEncoding = new UTF8Encoding(false);
            var defaultOutputTemplate = StringLogSerializer.DefaultOutputTemplate;
            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var outputTemplate = "{Message}";
            var serializer = new StringLogSerializer { OutputTemplate = outputTemplate };
            var serializerApply = new Action<StringLogSerializer>(s => { s.OutputTemplate = outputTemplate; });
            var encoding = Encoding.ASCII;
            var folder = Environment.CurrentDirectory;
            var fileNameProviderMock = new Mock<IRollingFileProvider>();
            fileNameProviderMock.SetupGet(x => x.FolderPath).Returns(folder);
            var fileNameProvider = fileNameProviderMock.Object;
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
            Assert.AreEqual(defaultMinLevel, logger1.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(defaultOutputTemplate, logger1.Serializer.CastTo<StringLogSerializer>().OutputTemplate);
            Assert.AreEqual(defaultEncoding, logger1.Encoding);
            Assert.AreEqual(folder, logger1.FileProvider.FolderPath);

            Assert.AreEqual(pipeCollection, logger2.Pipes);
            Assert.AreEqual(serializer, logger2.Serializer);
            Assert.AreEqual(encoding, logger2.Encoding);
            Assert.AreEqual(defaultMinLevel, logger2.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(folder, logger2.FileProvider.FolderPath);

            #endregion Assert
        }

        [Test]
        public void WizardLoggingExtensions_BackgroundLogger()
        {
            #region Arrange

            var minLevel = LogLevel.Warning;
            var levelVerifier = new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Warning);
            var baseLogger = new Mock<ILogger>().Object;
            var baseLoggerFactory = new Func<ILoggingWizardCharms, ILogger>(s => new Mock<ILogger>().Object);
            var flushPeriod = TimeSpan.FromSeconds(10);

            #endregion Arrange

            #region Act

            var logger1 = LoggingWizard.Spell.BackgroundLogger(baseLogger);
            var logger2 = LoggingWizard.Spell.BackgroundLogger(baseLogger, minLevel);
            var logger3 = LoggingWizard.Spell.BackgroundLogger(baseLogger, levelVerifier);
            var logger4 = LoggingWizard.Spell.BackgroundLogger(baseLoggerFactory, minLevel);
            var logger5 = LoggingWizard.Spell.BackgroundLogger(baseLoggerFactory, levelVerifier);
            var logger6 = LoggingWizard.Spell.BackgroundLogger(baseLogger, flushPeriod: flushPeriod);
            var logger7 = LoggingWizard.Spell.BackgroundLogger(baseLogger, minLevel, flushPeriod);
            var logger8 = LoggingWizard.Spell.BackgroundLogger(baseLogger, levelVerifier, flushPeriod);
            var logger9 = LoggingWizard.Spell.BackgroundLogger(baseLoggerFactory, minLevel, flushPeriod);
            var logger10 = LoggingWizard.Spell.BackgroundLogger(baseLoggerFactory, levelVerifier, flushPeriod);

            #endregion Act

            #region Assert

            #endregion Assert
        }
    }
}