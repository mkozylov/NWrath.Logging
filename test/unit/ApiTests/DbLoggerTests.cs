using Moq;
using Moq.Protected;
using NUnit.Framework;
using NWrath.Logging.Test.Structs;
using System;
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

            var connectionStub = new DbConnectionStub { ConnectionString = connectionStr };

            var schemaMock = new Mock<SqlLogSchema>(connectionStr, null, null, null) { CallBase = true };
            schemaMock.Setup(x => x.CreateConnection())
                      .Returns(connectionStub);

            var loggerMock = new Mock<DbLogger>(schemaMock.Object) { CallBase = true };
            var loggerMockProtected = loggerMock.Protected();

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

            Assert.AreEqual(connectionStr, loggerMock.Object.Schema.ConnectionString);
            AssertDefaultSchemaDbCommand(firstCommand, log1);
            AssertDefaultSchemaDbCommand(secondCommand, log2);
            loggerMockProtected.Verify("ExecuteInitScript", Times.Once());
            loggerMockProtected.Verify("WriteRecord", Times.Exactly(2), ItExpr.IsAny<LogRecord>());

            #endregion Assert
        }

        #region Internal

        private void AssertDefaultSchemaDbCommand(DbCommand cmd, LogRecord record)
        {
            var insertScript = $"INSERT INTO [{SqlLogSchema.DefaultTableName}]([Timestamp], [Message], [Exception], [Level]) VALUES("
                         + $"'{record.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}','{record.Message}',{(record.Exception == null ? "NULL" : record.Exception.ToString())},{(int)record.Level})";

            Assert.AreEqual(insertScript, cmd.CommandText);
        }

        #endregion Internal
    }
}