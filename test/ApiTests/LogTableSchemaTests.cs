using NUnit.Framework;
using System.Text;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LogTableSchemaTests
    {
        [Test]
        public void LogTableSchema_Build_Default()
        {
            #region Arrange

            var ts = new LogTableSchema();

            var initScript = new StringBuilder("IF OBJECT_ID(N'ServerLog', N'U') IS NULL BEGIN ")
                                        .Append("CREATE TABLE ServerLog(")
                                            .Append("Id BIGINT NOT NULL PRIMARY KEY IDENTITY,")
                                            .Append("Timestamp DATETIME NOT NULL,")
                                            .Append("Message VARCHAR(MAX) NOT NULL,")
                                            .Append("Level INT NOT NULL")
                                        .Append(") END")
                                        .ToString();

            var insertScript = "INSERT INTO ServerLog(Timestamp, Message, Level) VALUES(@Timestamp, @Message, @Level)";

            var defaultColumns = new[] {
                LogTableSchema.IdColumn,
                LogTableSchema.TimestampColumn,
                LogTableSchema.MessageColumn,
                LogTableSchema.LevelColumn
            };

            #endregion Arrange

            #region Act

            ts.Build();

            #endregion Act

            #region Assert

            Assert.AreEqual(LogTableSchema.DefaultTableName, ts.TableName);
            Assert.AreEqual(defaultColumns.Length, ts.GetColumns().Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_Build_CustomScripts()
        {
            #region Arrange

            var initScript = "INIT SCRIPT";

            var insertScript = "INSERT SCRIPT";

            var ts = new LogTableSchema(initScript: initScript, inserLogScript: insertScript);

            #endregion Arrange

            #region Act

            ts.Build();

            #endregion Act

            #region Assert

            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_Build_CustomTable()
        {
            #region Arrange

            var newTableName = "CustomLogTable";

            var ts = new LogTableSchema(newTableName);

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

            ts.Build();

            #endregion Act

            #region Assert

            Assert.AreEqual(newTableName, ts.TableName);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }

        [Test]
        public void LogTableSchema_Build_CustomColumns()
        {
            #region Arrange

            var newColumns = new[] {
                LogTableSchema.MessageColumn,
                LogTableSchema.ExceptionColumn
            };
            var ts = new LogTableSchema(columns: newColumns);

            var initScript = new StringBuilder("IF OBJECT_ID(N'ServerLog', N'U') IS NULL BEGIN ")
                                        .Append("CREATE TABLE ServerLog(")
                                            .Append("Message VARCHAR(MAX) NOT NULL,")
                                            .Append("Exception VARCHAR(MAX) NULL")
                                        .Append(") END")
                                        .ToString();

            var insertScript = "INSERT INTO ServerLog(Message, Exception) VALUES(@Message, @Exception)";

            #endregion Arrange

            #region Act

            ts.Build();

            #endregion Act

            #region Assert

            Assert.AreEqual(newColumns.Length, ts.GetColumns().Length);
            Assert.AreEqual(initScript, ts.InitScript);
            Assert.AreEqual(insertScript, ts.InserLogScript);

            #endregion Assert
        }
    }
}