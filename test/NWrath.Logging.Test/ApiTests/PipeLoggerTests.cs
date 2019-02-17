using Moq;
using NUnit.Framework;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class PipeLoggerTests
    {
        [Test]
        public void PipeLogger_Log()
        {
            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var pipedMsg = default(LogRecord);

            var levelVerifierMock = new Mock<ILogRecordVerifier>();
            levelVerifierMock.Setup(x => x.Verify(It.IsAny<LogRecord>())).Returns(true);

            var targetLoggerMock = new Mock<ILogger>();
            targetLoggerMock.SetupProperty(x => x.IsEnabled, true);
            targetLoggerMock.SetupProperty(x => x.RecordVerifier, levelVerifierMock.Object);
            targetLoggerMock.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => pipedMsg = m);

            var logger = new PipeLogger<ILogger>(targetLoggerMock.Object);

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreSame(msg, pipedMsg);

            #endregion Assert
        }

        [Test]
        public void PipeLogger_AddSimplePipe()
        {
            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var pipedMsg = default(LogRecord);

            var levelVerifierMock = new Mock<ILogRecordVerifier>();
            levelVerifierMock.Setup(x => x.Verify(It.IsAny<LogRecord>())).Returns(true);

            var targetLoggerMock = new Mock<ILogger>();
            targetLoggerMock.SetupProperty(x => x.IsEnabled, true);
            targetLoggerMock.SetupProperty(x => x.RecordVerifier, levelVerifierMock.Object);
            targetLoggerMock.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => pipedMsg = m);

            var logger = new PipeLogger<ILogger>(targetLoggerMock.Object);

            logger.Pipes.Add((ctx, next) =>
            {
                ctx.LogRecord.Message = ctx.LogRecord.Message.ToUpper();
            });

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(2, logger.Pipes.Count);
            Assert.AreSame(msg, pipedMsg);
            Assert.AreEqual(msg.Message.ToUpper(), pipedMsg.Message);

            #endregion Assert
        }

        [Test]
        public void PipeLogger_ClearPipeline()
        {
            #region Arrange

            var msg = LogRecord.Empty;
            var pipedMsg = default(LogRecord);

            var levelVerifierMock = new Mock<ILogRecordVerifier>();
            levelVerifierMock.Setup(x => x.Verify(It.IsAny<LogRecord>())).Returns(true);

            var targetLoggerMock = new Mock<ILogger>();
            targetLoggerMock.SetupProperty(x => x.IsEnabled, true);
            targetLoggerMock.SetupProperty(x => x.RecordVerifier, levelVerifierMock.Object);
            targetLoggerMock.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => pipedMsg = m);

            var logger = new PipeLogger<ILogger>(targetLoggerMock.Object);
            logger.Pipes.Clear();

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(0, logger.Pipes.Count);
            Assert.AreSame(default(LogRecord), pipedMsg);

            #endregion Assert
        }

        [Test]
        public void PipeLogger_AddPipeline()
        {
            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var pipedMsg = default(LogRecord);

            var pipe1Called = 0;
            var pipe2Called = 0;

            var levelVerifierMock = new Mock<ILogRecordVerifier>();
            levelVerifierMock.Setup(x => x.Verify(It.IsAny<LogRecord>())).Returns(true);

            var targetLoggerMock = new Mock<ILogger>();
            targetLoggerMock.SetupProperty(x => x.IsEnabled, true);
            targetLoggerMock.SetupProperty(x => x.RecordVerifier, levelVerifierMock.Object);
            targetLoggerMock.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => pipedMsg = m);

            var logger = new PipeLogger<ILogger>(targetLoggerMock.Object);
            logger.Pipes.AddRange(
                (ctx, next) => { pipe1Called++; ctx.LogRecord.Message = ctx.LogRecord.Message.ToUpper(); next(ctx); },
                (ctx, next) => { pipe2Called++; ctx.LogRecord.Level = LogLevel.Debug; next(ctx); }
                );

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(3, logger.Pipes.Count);
            Assert.AreEqual(1, pipe1Called);
            Assert.AreEqual(1, pipe2Called);
            Assert.AreSame(msg, pipedMsg);
            Assert.AreEqual(msg.Message.ToUpper(), pipedMsg.Message);
            Assert.AreEqual(LogLevel.Debug, pipedMsg.Level);

            #endregion Assert
        }

        [Test]
        public void PipeLogger_PipelineWithSkipping()
        {
            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var pipedMsg = default(LogRecord);

            var pipe1Called = 0;
            var pipe2Called = 0;

            var levelVerifierMock = new Mock<ILogRecordVerifier>();
            levelVerifierMock.Setup(x => x.Verify(It.IsAny<LogRecord>())).Returns(true);

            var targetLoggerMock = new Mock<ILogger>();
            targetLoggerMock.SetupProperty(x => x.IsEnabled, true);
            targetLoggerMock.SetupProperty(x => x.RecordVerifier, levelVerifierMock.Object);
            targetLoggerMock.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => pipedMsg = m);

            var logger = new PipeLogger<ILogger>(targetLoggerMock.Object);
            logger.Pipes.AddRange(
                (ctx, next) => { pipe1Called++; ctx.LogRecord.Message = ctx.LogRecord.Message.ToUpper(); },
                (ctx, next) => { pipe2Called++; ctx.LogRecord.Level = LogLevel.Debug; next(ctx); }
                );

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(3, logger.Pipes.Count);
            Assert.AreEqual(1, pipe1Called);
            Assert.AreEqual(0, pipe2Called);
            Assert.AreSame(msg, pipedMsg);
            Assert.AreEqual(msg.Message.ToUpper(), pipedMsg.Message);
            Assert.AreEqual(LogLevel.Error, pipedMsg.Level);

            #endregion Assert
        }
    }
}