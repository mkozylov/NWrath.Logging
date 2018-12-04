using Moq;
using Moq.Protected;
using NUnit.Framework;
using NWrath.Logging.Test.Structs;
using System.Data.Common;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class DbLoggerTests
    {
        [Test]
        public void DbLogger_Log()
        {
            #region Arrange

            var connectionStr = "connectionStr";
            var log1 = new LogRecord("log1");
            var log2 = new LogRecord("log2");
            var loggerMock = new Mock<DbLogger>(connectionStr) { CallBase = true };

            var loggerMockProtected = loggerMock.Protected();
            var connectionStub = new DbConnectionStub { ConnectionString = connectionStr };

            loggerMockProtected.Setup<DbConnection>("CreateConnection", ItExpr.IsAny<string>())
                           .Returns(connectionStub);

            var firstCommand = default(DbCommand);
            var secondCommand = default(DbCommand);

            #endregion Arrange

            #region Act

            loggerMock.Object.Log(log1);
            firstCommand = connectionStub.LastCommand;

            loggerMock.Object.Log(log2);
            secondCommand = connectionStub.LastCommand;

            #endregion Act

            #region Assert

            Assert.AreEqual(connectionStr, loggerMock.Object.ConnectionString);
            AssertDefaultSchemaDbCommand(firstCommand, log1);
            AssertDefaultSchemaDbCommand(secondCommand, log2);
            loggerMockProtected.Verify("ExecuteInitScript", Times.Once(), ItExpr.IsAny<string>());
            loggerMockProtected.Verify("WriteRecord", Times.Exactly(2), ItExpr.IsAny<LogRecord>());

            #endregion Assert
        }

        #region Internal

        private void AssertDefaultSchemaDbCommand(DbCommand cmd, LogRecord log)
        {
            Assert.AreEqual(3, cmd.Parameters.Count);

            var t = cmd.Parameters[0];
            var m = cmd.Parameters[1];
            var l = cmd.Parameters[2];

            Assert.AreEqual("@Timestamp", t.ParameterName);
            Assert.AreEqual("@Message", m.ParameterName);
            Assert.AreEqual("@Level", l.ParameterName);

            Assert.AreEqual($"{log.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}", t.Value.ToString());
            Assert.AreEqual(log.Message, m.Value);
            Assert.AreEqual(log.Level, l.Value);
        }

        #endregion Internal
    }
}