using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            var list = new List<LogMessage>();
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
                logger.Log(new LogMessage(i.ToString()));
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
    }
}