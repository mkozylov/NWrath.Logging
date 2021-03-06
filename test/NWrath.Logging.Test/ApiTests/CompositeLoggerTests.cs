﻿using Moq;
using NUnit.Framework;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class CompositeLoggerTests
    {
        [Test]
        public void CompositeLogger_NoSetLoggers_Throw()
        {
            var err = Errors.NO_LOGGERS;

            var loggers = new ILogger[] { new Mock<ILogger>().Object };

            Assert.Throws<ArgumentException>(() => new CompositeLogger(new ILogger[] { }), err.Message);
            Assert.Throws<ArgumentException>(() => new CompositeLogger(loggers).Loggers = new ILogger[] { }, err.Message);
        }

        [Test]
        public void CompositeLogger_SetLoggers_Success()
        {
            var loggers = new ILogger[] { new Mock<ILogger>().Object };

            Assert.DoesNotThrow(() => new CompositeLogger(loggers));
        }

        [Test]
        public void CompositeLogger_Log()
        {
            var msg = new LogRecord();
            var subMsg1 = default(LogRecord);
            var subMsg2 = default(LogRecord);

            var subLoggerMock1 = new Mock<ILogger>();
            subLoggerMock1.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => subMsg1 = m);

            var subLoggerMock2 = new Mock<ILogger>();
            subLoggerMock2.Setup(x => x.Log(It.IsAny<LogRecord>()))
                          .Callback<LogRecord>(m => subMsg2 = m);

            var logger = new CompositeLogger(new[] { subLoggerMock1.Object, subLoggerMock2.Object });

            logger.Log(msg);

            Assert.AreEqual(2, logger.Loggers.Length);
            Assert.AreEqual(msg, subMsg1);
            Assert.AreEqual(msg, subMsg2);
        }
    }
}