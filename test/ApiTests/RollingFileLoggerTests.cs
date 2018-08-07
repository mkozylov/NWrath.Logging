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

            var log = new LogMessage
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
            //one call by default writer initializer, one call by createFileAction, because file does not exists
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Exactly(2));
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Exactly(2));

            logFile.Delete();

            #endregion Assert
        }

        [Test]
        public void RollingFileLogger_CustomPipeline()
        {
            #region Arrange

            var currentFolder = Path.GetDirectoryName(GetType().Assembly.Location);

            var log = new LogMessage
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

            var logs = new List<LogMessage>();

            var pipes = new PipeCollection<RollingFileContext>()
                        .Add((c, n) =>
                        {
                            n(c);
                            logs.Add(c.LogMessage);
                        })
                        .Add((c, n) =>
                        {
                            c.LogMessage.Message = c.LogMessage.Message.ToUpper();
                            n(c);
                        });

            var logger = new RollingFileLogger(fileProviderMock.Object, pipes: pipes);

            #endregion Arrange

            #region Act

            fileBeforeWriteLogExists = new FileInfo(file).Exists;
            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            Assert.IsFalse(fileBeforeWriteLogExists);
            Assert.IsFalse(new FileInfo(file).Exists);
            Assert.AreEqual(1, logs.Count);
            Assert.AreSame(logs[0], log);
            Assert.AreEqual(logs[0].Message, log.Message.ToUpper());
            //one call by default writer initializer, because file does not exists
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Once());
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Once());

            #endregion Assert
        }
    }
}