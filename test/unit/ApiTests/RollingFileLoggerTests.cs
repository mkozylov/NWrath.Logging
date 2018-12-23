using Moq;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class RollingFileLoggerTests
    {
        [Test]
        public void RollingFileLogger_Log()
        {
            #region Arrange

            var currentFolder = Path.GetDirectoryName(GetType().Assembly.Location);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var file = $"{currentFolder}\\log.txt";
            var files = new List<string> { file };
            var fileBeforeWriteLogExists = false;

            new FileInfo(file).If(x => x.Exists, t => t.Delete());

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.FolderPath).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(file);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                files.Add(file);
                return file;
            });

            var logger = new RollingFileLogger(fileProviderMock.Object);

            #endregion Arrange

            #region Act

            new FileInfo(file)
                .If(f => f.Exists, t => t.Delete());

            fileBeforeWriteLogExists = new FileInfo(file).Exists;
            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            var logFile = new FileInfo(file);
            var expectedStr = logger.Serializer.Serialize(log) + Environment.NewLine;

            Assert.IsFalse(fileBeforeWriteLogExists);
            Assert.IsTrue(logFile.Exists);
            Assert.AreEqual(expectedStr, File.ReadAllText(file));

            //1 default writer; 1 create file action; 1 clear action
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Exactly(3));
            //1 default writer; 1 create file action
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Exactly(2));

            logFile.Delete();

            #endregion Assert
        }

        [Test]
        public void RollingFileLogger_CustomPipeline()
        {
            #region Arrange

            var currentFolder = Path.GetDirectoryName(GetType().Assembly.Location);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var file = $"{currentFolder}\\log.txt";
            var files = new List<string> { file };
            var fileBeforeWriteLogExists = false;

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.FolderPath).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(file);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                files.Add(file);
                return file;
            });

            var logs = new List<LogRecord>();

            var pipes = new PipeCollection<RollingFileContext>()
                        .Add((c, n) =>
                        {
                            n(c);
                            logs.Add(c.LogRecord);
                        })
                        .Add((c, n) =>
                        {
                            c.LogRecord.Message = c.LogRecord.Message.ToUpper();
                            n(c);
                        });

            pipes.Insert(0, RollingFileLogger.LogWriterPipe);

            var logger = new RollingFileLogger(fileProviderMock.Object) { Pipes = pipes };

            #endregion Arrange

            #region Act

            new FileInfo(file)
                .If(f => f.Exists, t => t.Delete());

            fileBeforeWriteLogExists = new FileInfo(file).Exists;
            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            Assert.IsFalse(fileBeforeWriteLogExists);
            Assert.IsTrue(new FileInfo(file).Exists);
            Assert.AreEqual(1, logs.Count);
            Assert.AreSame(logs[0], log);
            Assert.AreEqual(logs[0].Message, log.Message.ToUpper());
            //one call by default writer initializer, because file does not exists
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Once());
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Once());

            #endregion Assert
        }

        [Test]
        public void RollingFileLogger_Log2()
        {
            #region Arrange

            var currentFolder = Path.GetDirectoryName(GetType().Assembly.Location);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var file = $"{currentFolder}\\log.txt";
            var files = new List<string> { file };
            var fileBeforeWriteLogExists = false;

            new FileInfo(file).If(x => x.Exists, t => t.Delete());

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.FolderPath).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(file);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                files.Add(file);
                return file;
            });

            var logger = LoggingWizard.Spell.RollingFileLogger(fileProviderMock.Object, serializerApply: s =>
            {
                s.OutputTemplate += "{Ul}";
                s.Formats["Ul"] = r => "\n" + new string('-', 50);
            });

            #endregion Arrange

            #region Act

            new FileInfo(file)
                .If(f => f.Exists, t => t.Delete());

            fileBeforeWriteLogExists = new FileInfo(file).Exists;
            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            var logFile = new FileInfo(file);
            var expectedStr = logger.Serializer.Serialize(log) + Environment.NewLine;

            Assert.IsFalse(fileBeforeWriteLogExists);
            Assert.IsTrue(logFile.Exists);
            var str = File.ReadAllText(file);
            Assert.AreEqual(expectedStr, str);

            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Exactly(3));
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Exactly(2));

            logFile.Delete();

            #endregion Assert
        }
    }
}