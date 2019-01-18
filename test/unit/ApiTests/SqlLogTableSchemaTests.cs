﻿using NUnit.Framework;
using System;
using System.Text;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class SqlLogTableSchemaTests
    {
        [Test]
        public void LogTableSchema_Default()
        {
            #region Arrange

            var initScript = new StringBuilder($"IF OBJECT_ID(N'[{SqlLogTableSchema.DefaultTableName}]', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE [{SqlLogTableSchema.DefaultTableName}](")
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

            var insertScript = $"INSERT INTO [{SqlLogTableSchema.DefaultTableName}]([Timestamp], [Message], [Exception], [Level]) VALUES("
                             + $"'{record.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}','{record.Message}','{record.Exception.ToString()}',{(int)record.Level})";

            var defaultColumns = new[] {
                SqlLogTableSchema.IdColumn,
                SqlLogTableSchema.TimestampColumn,
                SqlLogTableSchema.MessageColumn,
                SqlLogTableSchema.ExceptionColumn,
                SqlLogTableSchema.LevelColumn
            };

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema();
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(SqlLogTableSchema.DefaultTableName, ts.TableName);
            Assert.AreEqual(defaultColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomInitScript()
        {
            #region Arrange

            var initScript = "INIT SCRIPT";

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema(initScript: initScript);

            #endregion Act

            #region Assert

            Assert.AreEqual(initScript, ts.InitScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomTable()
        {
            #region Arrange

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

            var ts = new SqlLogTableSchema(newTableName);
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(newTableName, ts.TableName);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomColumns()
        {
            #region Arrange

            var newColumns = new[] {
                SqlLogTableSchema.MessageColumn,
                SqlLogTableSchema.ExceptionColumn
            };

            var initScript = new StringBuilder($"IF OBJECT_ID(N'[{SqlLogTableSchema.DefaultTableName}]', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE [{SqlLogTableSchema.DefaultTableName}](")
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

            var insertScript = $"INSERT INTO [{SqlLogTableSchema.DefaultTableName}]([Message], [Exception]) VALUES("
                             + $"'{record.Message}','{record.Exception.ToString()}')";

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema(columns: newColumns);
            var insertQuery = ts.BuildInsertQuery(record);

            #endregion Act

            #region Assert

            Assert.AreEqual(newColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, insertQuery);

            #endregion Assert
        }
    }
}