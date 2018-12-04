using Moq;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class ThreadSafeLoggerTests
    {
        [Test]
        public void ThreadSafeLogger_OptionsPropagation()
        {
            var verifier = new MinimumLogLevelVerifier(LogLevel.Debug);
            var oldSaveVerifier = default(ILogLevelVerifier);

            var loggerMock = new Mock<ILogger>()
                .SetupProperty(x => x.IsEnabled, true)
                .SetupProperty(x => x.LevelVerifier, verifier);

            var saveLogger = new ThreadSafeLogger(loggerMock.Object);
            oldSaveVerifier = saveLogger.LevelVerifier;
            saveLogger.LevelVerifier = new MinimumLogLevelVerifier(LogLevel.Error);
            saveLogger.IsEnabled = false;

            Assert.AreEqual(verifier, oldSaveVerifier);
            Assert.AreNotEqual(oldSaveVerifier, saveLogger.LevelVerifier);
            Assert.IsInstanceOf<MinimumLogLevelVerifier>(saveLogger.LevelVerifier);
            Assert.AreEqual(LogLevel.Error, saveLogger.LevelVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.AreEqual(false, loggerMock.Object.IsEnabled);
        }

        [Test]
        public void ThreadSafeLogger_SaveMultiThreadLog()
        {
            var messages = new List<string>();

            var logger = new LambdaLogger(m => { messages.Add(m.Message); });

            var saveLogger = new ThreadSafeLogger(logger);

            var task1 = Task.Run(() =>
            {
                Enumerable.Range(1, 1500).Each(x =>
                {
                    saveLogger.Log(new LogRecord { Message = "1" });
                });
            });
            var task2 = Task.Run(() =>
            {
                Enumerable.Range(1, 1500).Each(x =>
                {
                    saveLogger.Log(new LogRecord { Message = "2" });
                });
            });

            Task.WaitAll(task1, task2);

            Assert.AreEqual(3000, messages.Count);
            Assert.AreEqual(messages.Count(x => x == "1"), messages.Count(x => x == "2"));
        }

        [Test]
        public void ThreadSafeLogger_NotSaveMultiThreadLog()
        {
            var messages = new List<string>();

            var logger = new LambdaLogger(m => { messages.Add(m.Message); });

            var task1 = Task.Run(() =>
            {
                Enumerable.Range(1, 1500).Each(x =>
                {
                    logger.Log(new LogRecord { Message = "1" });
                });
            });
            var task2 = Task.Run(() =>
            {
                Enumerable.Range(1, 1500).Each(x =>
                {
                    logger.Log(new LogRecord { Message = "2" });
                });
            });

            var err = default(Exception);

            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (Exception ex)
            {
                err = ex;
            }

            Assert.AreNotEqual(3000, messages.Count);
            Assert.IsTrue(err == null || err.ToString().Contains("Index was outside the bounds of the array"));
        }
    }
}