using NUnit.Framework;
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

            var initScript = new StringBuilder($"IF OBJECT_ID(N'{SqlLogTableSchema.DefaultTableName}', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE {SqlLogTableSchema.DefaultTableName}(")
                                            .Append("Id BIGINT NOT NULL PRIMARY KEY IDENTITY,")
                                            .Append("Timestamp DATETIME NOT NULL,")
                                            .Append("Message VARCHAR(MAX) NOT NULL,")
                                            .Append("Level INT NOT NULL")
                                        .Append(") END")
                                        .ToString();

            var insertScript = $"INSERT INTO {SqlLogTableSchema.DefaultTableName}(Timestamp, Message, Level) VALUES(@Timestamp, @Message, @Level)";

            var defaultColumns = new[] {
                SqlLogTableSchema.IdColumn,
                SqlLogTableSchema.TimestampColumn,
                SqlLogTableSchema.MessageColumn,
                SqlLogTableSchema.LevelColumn
            };

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema();

            #endregion Act

            #region Assert

            Assert.AreEqual(SqlLogTableSchema.DefaultTableName, ts.TableName);
            Assert.AreEqual(defaultColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomScripts()
        {
            #region Arrange

            var initScript = "INIT SCRIPT";

            var insertScript = "INSERT SCRIPT";

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema(initScript: initScript, inserLogScript: insertScript);

            #endregion Act

            #region Assert

            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_CustomTable()
        {
            #region Arrange

            var newTableName = "CustomLogTable";

            var initScript = new StringBuilder($"IF OBJECT_ID(N'{newTableName}', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE {newTableName}(")
                                            .Append("Id BIGINT NOT NULL PRIMARY KEY IDENTITY,")
                                            .Append("Timestamp DATETIME NOT NULL,")
                                            .Append("Message VARCHAR(MAX) NOT NULL,")
                                            .Append("Level INT NOT NULL")
                                        .Append(") END")
                                        .ToString();

            var insertScript = $"INSERT INTO {newTableName}(Timestamp, Message, Level) VALUES(@Timestamp, @Message, @Level)";

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema(newTableName);

            #endregion Act

            #region Assert

            Assert.AreEqual(newTableName, ts.TableName);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

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

            var initScript = new StringBuilder($"IF OBJECT_ID(N'{SqlLogTableSchema.DefaultTableName}', N'U') IS NULL BEGIN ")
                                        .Append($"CREATE TABLE {SqlLogTableSchema.DefaultTableName}(")
                                            .Append("Message VARCHAR(MAX) NOT NULL,")
                                            .Append("Exception VARCHAR(MAX) NULL")
                                        .Append(") END")
                                        .ToString();

            var insertScript = $"INSERT INTO {SqlLogTableSchema.DefaultTableName}(Message, Exception) VALUES(@Message, @Exception)";

            #endregion Arrange

            #region Act

            var ts = new SqlLogTableSchema(columns: newColumns);

            #endregion Act

            #region Assert

            Assert.AreEqual(newColumns.Length, ts.Columns.Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }
    }
}