using Moq;
using Moq.Protected;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LoggerBaseTests
    {
        [Test]
        public void LoggerBase_LogDebug()
        {
            const string msg = "str";
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Debug(msg);

            AssertLogWithLevel(loggerMock, msg, LogLevel.Debug, target);
        }

        [Test]
        public void LoggerBase_LogInfo()
        {
            const string msg = "str";
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Info(msg);

            AssertLogWithLevel(loggerMock, msg, LogLevel.Info, target);
        }

        [Test]
        public void LoggerBase_LogWarning()
        {
            const string msg = "str";
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Warning(msg);

            AssertLogWithLevel(loggerMock, msg, LogLevel.Warning, target);
        }

        [Test]
        public void LoggerBase_LogError()
        {
            const string msg = "str";
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Error(msg);

            AssertLogWithLevel(loggerMock, msg, LogLevel.Error, target);
        }

        [Test]
        public void LoggerBase_LogCritical()
        {
            const string msg = "str";
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Critical(msg);

            AssertLogWithLevel(loggerMock, msg, LogLevel.Critical, target);
        }

        [Test]
        public void LoggerBase_Log()
        {
            var msg = new LogRecord { Message = "str" };
            var target = default(LogRecord);
            var loggerMock = CreateLoggerMock(writeLogCallback: x => target = x);

            loggerMock.Object.Log(msg);

            AssertLogWithLevel(loggerMock, msg.Message, msg.Level, target);
            Assert.AreEqual(msg, target);
        }

        [Test]
        public void LoggerBase_LogWithVerifiedLevel()
        {
            var loggerMock = CreateLoggerMock(verifierReturns: true);

            loggerMock.Object.Log(new LogRecord());

            VerifyLogCall(loggerMock, logCallTimes: Times.Once(), writeLogCallTimes: Times.Once());
        }

        [Test]
        public void LoggerBase_LogWithNotVerifiedLevel()
        {
            var loggerMock = CreateLoggerMock(verifierReturns: false);

            loggerMock.Object.Log(new LogRecord());

            VerifyLogCall(loggerMock, logCallTimes: Times.Once(), writeLogCallTimes: Times.Never());
        }

        [Test]
        public void LoggerBase_LogWithEnabledLogger()
        {
            var loggerMock = CreateLoggerMock(loggerEnabled: true);

            loggerMock.Object.Log(new LogRecord());

            VerifyLogCall(loggerMock, logCallTimes: Times.Once(), writeLogCallTimes: Times.Once());
        }

        [Test]
        public void LoggerBase_LogWithDisabledLogger()
        {
            var loggerMock = CreateLoggerMock(loggerEnabled: false);

            loggerMock.Object.Log(new LogRecord());

            VerifyLogCall(loggerMock, logCallTimes: Times.Once(), writeLogCallTimes: Times.Never());
        }

        #region Internal

        private Mock<LoggerBase> CreateLoggerMock(
           bool loggerEnabled = true,
           bool verifierReturns = true,
           Action<LogRecord> writeLogCallback = null
           )
        {
            writeLogCallback = writeLogCallback ?? (x => { });

            var verifierMock = new Mock<ILogLevelVerifier>()
                                  .Apply(x => x.Setup(s => s.Verify(It.IsAny<LogLevel>())).Returns(verifierReturns));

            var loggerMock = new Mock<LoggerBase>()
                               .Apply(x => x.CallBase = true)
                               .SetupProperty(x => x.IsEnabled, loggerEnabled)
                               .SetupProperty(x => x.LevelVerifier, verifierMock.Object)
                               .Apply(x => x.Protected()
                                            .Setup("WriteRecord", ItExpr.IsAny<LogRecord>())
                                            .Callback(writeLogCallback));

            return loggerMock;
        }

        private void VerifyLogCall(Mock<LoggerBase> loggerMock, Times logCallTimes, Times writeLogCallTimes)
        {
            loggerMock.Verify(x => x.Log(It.IsAny<LogRecord>()), logCallTimes);
            loggerMock.Protected().Verify("WriteRecord", writeLogCallTimes, ItExpr.IsAny<LogRecord>());
        }

        private void AssertLogWithLevel(Mock<LoggerBase> loggerMock, string expectedMsg, LogLevel expectedLevel, LogRecord target)
        {
            VerifyLogCall(loggerMock, logCallTimes: Times.Once(), writeLogCallTimes: Times.Once());
            Assert.NotNull(target);
            Assert.AreEqual(expectedLevel, target.Level);
            Assert.AreEqual(expectedMsg, target.Message);
        }

        #endregion Internal
    }
}