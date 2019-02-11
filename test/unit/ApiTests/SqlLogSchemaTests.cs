using NUnit.Framework;
using System;
using System.Text;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class SqlLogSchemaTests
    {
        [Test]
        public void LogTableSchema_Default()
        {
            #region Arrange

            var connectionString = "CONNECTION STRING";

            var initScript = new StringBuilder($"IF OBJECT_ID(N'[{SqlLogSchema.DefaultTableName}]', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE [{SqlLogSchema.DefaultTableName}](")
                                            .Append("[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,")
                                            .Append("[Timestamp] DATETIME NOT NULL,")
                                            .Append("[Message] VARCHAR(MAX) NOT NULL,")
                                            .Append("[Exception] VARCHAR(MAX) NULL,")
                                            .Append("[Level] INT NOT NULL")
                                        .Append(") END")
                                        .ToString();

            var record = new LogRecord(
                "msg",
                new System.DateTime(2000, 1, 2, 3, 4, 5),
                LogLevel.Info,
                new NotImplementedException()
                );

            var insertScript = $"INSERT INTO [{SqlLogSchema.DefaultTableName}]([Timestamp], [Message], [Exception], [Level]) VALUES("
                             + $"'{record.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}','{record.Message}','{record.Exception.ToString()}',{(int)record.Level})";

            var defaultColumns = new[] {
                SqlLogSchema.IdColumn,
                SqlLogSchema.TimestampColumn,
                SqlLogSchema.MessageColumn,
                SqlLogSchema.ExceptionColumn,
                SqlLogSchema.LevelColumn
            };

            #endregion Arrange

            #region Act

            var ts = new SqlLogSchema(connectionString);
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(connectionString, ts.ConnectionString);
            Assert.AreEqual(SqlLogSchema.DefaultTableName, ts.TableName);
            Assert.AreEqual(defaultColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomInitScript()
        {
            #region Arrange

            var connectionString = "CONNECTION STRING";
            var initScript = "INIT SCRIPT";

            #endregion Arrange

            #region Act

            var ts = new SqlLogSchema(connectionString, initScript: initScript);

            #endregion Act

            #region Assert

            Assert.AreEqual(connectionString, ts.ConnectionString);
            Assert.AreEqual(initScript, ts.InitScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomTable()
        {
            #region Arrange

            var connectionString = "CONNECTION STRING";
            var newTableName = "CustomLogTable";

            var initScript = new StringBuilder($"IF OBJECT_ID(N'[{newTableName}]', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE [{newTableName}](")
                                            .Append("[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,")
                                            .Append("[Timestamp] DATETIME NOT NULL,")
                                            .Append("[Message] VARCHAR(MAX) NOT NULL,")
                                            .Append("[Exception] VARCHAR(MAX) NULL,")
                                            .Append("[Level] INT NOT NULL")
                                        .Append(") END")
                                        .ToString();

            var record = new LogRecord(
                "msg",
                new System.DateTime(2000, 1, 2, 3, 4, 5),
                LogLevel.Info,
                new NotImplementedException()
                );

            var insertScript = $"INSERT INTO [{newTableName}]([Timestamp], [Message], [Exception], [Level]) VALUES("
                             + $"'{record.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}','{record.Message}','{record.Exception.ToString()}',{(int)record.Level})";

            #endregion Arrange

            #region Act

            var ts = new SqlLogSchema(connectionString, tableName: newTableName);
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(connectionString, ts.ConnectionString);
            Assert.AreEqual(newTableName, ts.TableName);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomColumns()
        {
            #region Arrange

            var connectionString = "CONNECTION STRING";

            var newColumns = new[] {
                SqlLogSchema.MessageColumn,
                SqlLogSchema.ExceptionColumn
            };

            var initScript = new StringBuilder($"IF OBJECT_ID(N'[{SqlLogSchema.DefaultTableName}]', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE [{SqlLogSchema.DefaultTableName}](")
                                            .Append("[Message] VARCHAR(MAX) NOT NULL,")
                                            .Append("[Exception] VARCHAR(MAX) NULL")
                                        .Append(") END")
                                        .ToString();

            var record = new LogRecord(
                "msg",
                new System.DateTime(2000, 1, 2, 3, 4, 5),
                LogLevel.Info,
                new NotImplementedException()
                );

            var insertScript = $"INSERT INTO [{SqlLogSchema.DefaultTableName}]([Message], [Exception]) VALUES("
                             + $"'{record.Message}','{record.Exception.ToString()}')";

            #endregion Arrange

            #region Act

            var ts = new SqlLogSchema(connectionString, columns: newColumns);
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(connectionString, ts.ConnectionString);
            Assert.AreEqual(newColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }
    }
}