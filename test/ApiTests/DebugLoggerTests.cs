using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class DebugLoggerTests
    {
        [Test]
        public void DebugLogger_Log()
        {
            #region Arrange

            var msg = new LogMessage();
            var serializer = new StringLogSerializer();
            var logger = new DebugLogger(serializer);
            var writer = new StringWriter();
            var trace = new TextWriterTraceListener(writer);
            Debug.Listeners.Add(trace);
            var exprctedMsg = serializer.Serialize(msg) + writer.NewLine;

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(exprctedMsg, writer.ToString());

            #endregion Assert
        }
    }
}