using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class BackgroundLoggerTests
    {
        [Test]
        public void BackgroundLogger_Log()
        {
            #region Arrange

            var expectedCount = 100;
            var list = new List<LogRecord>();
            var writeDelayCts = new CancellationTokenSource();

            var lambdaLogger = new LambdaLogger(async m =>
            {
                try
                {
                    await Task.Delay(-1, writeDelayCts.Token)
                              .ConfigureAwait(false);
                }
                catch (TaskCanceledException) { }

                list.Add(m);
            });

            var logger = new BackgroundLogger(lambdaLogger);

            foreach (var i in Enumerable.Range(1, expectedCount))
            {
                logger.Log(new LogRecord(i.ToString()));
            }

            #endregion Arrange

            #region Act

            var beforeCancelListCount = list.Count;
            writeDelayCts.Cancel();
            logger.Dispose();

            #endregion Act

            #region Assert

            Assert.AreEqual(0, beforeCancelListCount);
            Assert.AreEqual(expectedCount, list.Count);

            #endregion Assert
        }

        [Test]
        public void BackgroundLogger_LogRandomSequenceWriteSpeed()
        {
            #region Arrange

            var expectedCount = 100;
            var expectedSequence = Enumerable.Range(1, expectedCount);
            var list = new List<LogRecord>();
            var writeDelayCts = new CancellationTokenSource();

            var lambdaLogger = new LambdaLogger(m =>
            {
                try
                {
                    Task.Delay(-1, writeDelayCts.Token)
                        .Wait();
                }
                catch (Exception) { }

                Task.Delay(new Random(m.GetHashCode()).Next(50)).Wait();

                list.Add(m);
            });

            var logger = new BackgroundLogger(lambdaLogger);

            foreach (var i in Enumerable.Range(1, expectedCount))
            {
                logger.Log(new LogRecord(i.ToString()));
            }

            #endregion Arrange

            #region Act

            var beforeCancelListCount = list.Count;
            writeDelayCts.Cancel();
            logger.Dispose();

            #endregion Act

            #region Assert

            Assert.AreEqual(0, beforeCancelListCount);
            Assert.AreEqual(expectedCount, list.Count);
            Assert.True(list.Select(x => int.Parse(x.Message)).SequenceEqual(expectedSequence));

            #endregion Assert
        }
    }
}