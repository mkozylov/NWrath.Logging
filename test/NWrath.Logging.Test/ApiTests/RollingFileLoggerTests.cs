using Moq;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions.Collections;
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

            var currentFolderPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var currentFolder = new DirectoryInfo(currentFolderPath);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var filePathTemplate = $"{currentFolderPath}\\log.txt";
            var lastFileMock = new Mock<FileInformation>(filePathTemplate);
            lastFileMock.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
            lastFileMock.SetupGet(x => x.FullName).Returns(filePathTemplate);
            lastFileMock.SetupGet(x => x.Exists).Returns(true);

            var files = new List<FileInformation> { lastFileMock.Object };

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.Directory).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(lastFileMock.Object);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                var newFilePath = filePathTemplate.Replace(".txt", (files.Count + 1) + ".txt");

                var fm = new Mock<FileInformation>(newFilePath);
                fm.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
                fm.SetupGet(x => x.FullName).Returns(newFilePath);
                fm.SetupGet(x => x.Exists).Returns(false);

                files.Add(fm.Object);

                return fm.Object;
            });

            var logger = new RollingFileLogger(fileProviderMock.Object);

            #endregion Arrange

            #region Act

            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            var logFile = new FileInfo(lastFileMock.Object.FullName);
            var expectedStr = logger.Serializer.Serialize(log) + Environment.NewLine;

            Assert.IsTrue(logFile.Exists);
            Assert.AreEqual(expectedStr, File.ReadAllText(logFile.FullName));

            //because last file exists
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Never);

            //1 default writer; 1 create file action
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Exactly(2));

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            #endregion Assert
        }

        [Test]
        public void RollingFileLogger_CustomPipeline()
        {
            #region Arrange

            var currentFolderPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var currentFolder = new DirectoryInfo(currentFolderPath);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var filePathTemplate = $"{currentFolderPath}\\log.txt";
            var lastFileMock = new Mock<FileInformation>(filePathTemplate);
            lastFileMock.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
            lastFileMock.SetupGet(x => x.FullName).Returns(filePathTemplate);
            lastFileMock.SetupGet(x => x.Exists).Returns(true);

            var files = new List<FileInformation> { lastFileMock.Object };

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.Directory).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(lastFileMock.Object);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                var newFilePath = filePathTemplate.Replace(".txt", (files.Count + 1) + ".txt");

                var fm = new Mock<FileInformation>(newFilePath);
                fm.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
                fm.SetupGet(x => x.FullName).Returns(newFilePath);
                fm.SetupGet(x => x.Exists).Returns(false);

                files.Add(fm.Object);

                return fm.Object;
            });

            var logs = new List<LogRecord>();

            var pipes = new PipeCollection<RollingFileContext>()
                        .Add(RollingFileLogger.LogWriterPipe)
                        .Add((c, n) =>
                        {
                            n(c);
                            logs.AddRange(c.Batch);
                        })
                        .Add((c, n) =>
                        {
                            c.Batch.Each(x => x.Message = x.Message.ToUpper());
                            n(c);
                        });

            var logger = new RollingFileLogger(fileProviderMock.Object) { Pipes = pipes };

            #endregion Arrange

            #region Act

            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            var logFile = new FileInfo(lastFileMock.Object.FullName);

            Assert.IsTrue(logFile.Exists);
            Assert.AreEqual(1, logs.Count);
            Assert.AreSame(logs[0], log);
            Assert.AreEqual(logs[0].Message, log.Message.ToUpper());
            //because last file exists
            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Never());
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Once());

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            #endregion Assert
        }

        [Test]
        public void RollingFileLogger_LogWithCustomFormat()
        {
            #region Arrange

            var currentFolderPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var currentFolder = new DirectoryInfo(currentFolderPath);

            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var filePathTemplate = $"{currentFolderPath}\\log.txt";
            var lastFileMock = new Mock<FileInformation>(filePathTemplate);
            lastFileMock.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
            lastFileMock.SetupGet(x => x.FullName).Returns(filePathTemplate);
            lastFileMock.SetupGet(x => x.Exists).Returns(true);

            var files = new List<FileInformation> { lastFileMock.Object };

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            var fileProviderMock = new Mock<IRollingFileProvider>();
            fileProviderMock.Setup(x => x.GetFiles()).Returns(() => files.ToArray());
            fileProviderMock.Setup(x => x.Directory).Returns(currentFolder);
            fileProviderMock.Setup(x => x.TryResolveLastFile()).Returns(lastFileMock.Object);
            fileProviderMock.Setup(x => x.ProduceNewFile()).Returns(() =>
            {
                var newFilePath = filePathTemplate.Replace(".txt", (files.Count + 1) + ".txt");

                var fm = new Mock<FileInformation>(newFilePath);
                fm.SetupGet(x => x.CreationTime).Returns(DateTime.Now);
                fm.SetupGet(x => x.FullName).Returns(newFilePath);
                fm.SetupGet(x => x.Exists).Returns(false);

                files.Add(fm.Object);

                return fm.Object;
            });

            var logger = LoggingWizard.Spell.RollingFileLogger(fileProviderMock.Object, serializerApply: s =>
            {
                s.OutputTemplate += "{Ul}";
                s.Formats["Ul"] = r => "\n" + new string('-', 50);
            });

            #endregion Arrange

            #region Act

            logger.Log(log);
            logger.Dispose();

            #endregion Act

            #region Assert

            var logFile = new FileInfo(lastFileMock.Object.FullName);
            var expectedStr = logger.Serializer.Serialize(log) + Environment.NewLine;

            Assert.IsTrue(logFile.Exists);
            Assert.AreEqual(expectedStr, File.ReadAllText(logFile.FullName));

            fileProviderMock.Verify(x => x.ProduceNewFile(), Times.Never());
            fileProviderMock.Verify(x => x.TryResolveLastFile(), Times.Exactly(2));

            new DirectoryInfo(currentFolderPath).GetFiles("*.txt").Each(x => x.Delete());

            #endregion Assert
        }
    }
}