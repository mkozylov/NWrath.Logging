using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class StreamLoggerTests
    {
        [Test]
        public void StreamLogger_Log()
        {
            #region Arrange

            var msg = new LogRecord();
            var writer = new MemoryStream();
            var serializer = new StringLogSerializer();
            var logger = new StreamLogger(writer) { Serializer = serializer };
            var expectedMsg = serializer.Serialize(msg) + Environment.NewLine;
            var beforeCanWrite = logger.Writer.CanWrite;

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(expectedMsg, logger.Encoding.GetString(writer.ToArray()));
            Assert.AreEqual(beforeCanWrite, logger.Writer.CanWrite);

            #endregion Assert
        }

        [Test]
        public void StreamLogger_Dispose()
        {
            #region Arrange

            var writer = new MemoryStream();
            var logger = new StreamLogger(writer);
            var beforeCanWrite = logger.Writer.CanWrite;

            #endregion Arrange

            #region Act

            using (logger)
            {
                logger.Log(LogRecord.Empty);
            }

            #endregion Act

            #region Assert

            Assert.IsFalse(logger.Writer.CanWrite);
            Assert.AreNotEqual(beforeCanWrite, writer.CanWrite);

            #endregion Assert
        }

        [Test]
        public void StreamLogger_DestructorDispose()
        {
            #region Arrange

            var writer = new MemoryStream();
            var logger = new StreamLogger(writer);
            var beforeCanWrite = logger.Writer.CanWrite;

            #endregion Arrange

            #region Act

            logger.Log(LogRecord.Empty);
            logger = null;
            GC.Collect();
            //Need wait for destructor call
            Task.Delay(50).Wait();

            #endregion Act

            #region Assert

            Assert.AreNotEqual(beforeCanWrite, writer.CanWrite);

            #endregion Assert
        }
    }
}