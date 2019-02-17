using Moq;
using NUnit.Framework;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LambdaLoggerTests
    {
        [Test]
        public void LambdaLogger_Log()
        {
            var writeActionMock = new Mock<Action<LogRecord>>();
            writeActionMock.Setup(x => x(It.IsAny<LogRecord>()));

            var logger = new LambdaLogger(writeActionMock.Object);
            logger.Log(new LogRecord());

            Assert.AreEqual(logger.WriteAction, writeActionMock.Object);
            writeActionMock.Verify(x => x(It.IsAny<LogRecord>()));
        }
    }
}