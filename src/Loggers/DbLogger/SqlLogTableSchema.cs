using System;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class SqlLogTableSchema
        : ILogTableSchema
    {
        #region DefaultColumns

        public static LogTableColumnSchema IdColumn => new LogTableColumnSchema
        (
            name: "Id",
            typeDefinition: "BIGINT NOT NULL PRIMARY KEY IDENTITY",
            isInternal: true
        );

        public static LogTableColumnSchema TimestampColumn => new LogTableColumnSchema
        (
            name: "Timestamp",
            typeDefinition: "DATETIME NOT NULL",
            isInternal: false,
            serializer: new LambdaLogSerializer(m => m.Timestamp.ToIsoString('T'))
        );

        public static LogTableColumnSchema MessageColumn => new LogTableColumnSchema
        (
            name: "Message",
            typeDefinition: "VARCHAR(MAX) NOT NULL",
            isInternal: false,
            serializer: new LambdaLogSerializer(m => m.Message)
        );

        public static LogTableColumnSchema ExceptionColumn => new LogTableColumnSchema
        (
            name: "Exception",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            serializer: new LambdaLogSerializer(m => m.Exception == null ? null : m.Exception.ToString())
        );

        public static LogTableColumnSchema LevelColumn => new LogTableColumnSchema
        (
            name: "Level",
            typeDefinition: "INT NOT NULL",
            isInternal: false,
            serializer: new LambdaLogSerializer(m => m.Level)
        );

        public static LogTableColumnSchema ExtraColumn => new LogTableColumnSchema
        (
            name: "Extra",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            serializer: new LambdaLogSerializer(m => (m.Extra.Count == 0 ? null : m.Extra.AsJson()))
        );

        #endregion DefaultColumns

        public const string DefaultTableName = "ServerLog";

        public static LogTableColumnSchema[] DefaultColumns => GetDefaultColumns();

        public string TableName { get; private set; }

        public string InserLogScript { get; private set; }

        public string InitScript { get; private set; }

        public LogTableColumnSchema[] Columns { get; private set; }

        public SqlLogTableSchema(
            string tableName = DefaultTableName,
            string initScript = null,
            string inserLogScript = null,
            LogTableColumnSchema[] columns = null
            )
        {
            Columns = columns?.Length > 0 ? columns
                                          : DefaultColumns;
            TableName = tableName ?? DefaultTableName;
            InitScript = initScript ?? BuildDefaultInitScript();
            InserLogScript = inserLogScript ?? BuildDefaultInserLogScript();
        }

        public SqlLogTableSchema(LogTableSchemaConfig config)
            : this(config.TableName, config.InitScript, config.InserLogScript, config.Columns)
        {
        }

        private string BuildDefaultInserLogScript()
        {
            var columnNames = Columns.Where(x => !x.IsInternal)
                                     .Select(x => x.Name)
                                     .ToList();

            var columnsStr = string.Join(", ", columnNames.Select(x => "[" + x + "]"));
            var valsStr = string.Join(", ", columnNames.Select(x => "@" + x));

            return $"INSERT INTO [{TableName}]({columnsStr}) VALUES({valsStr})";
        }

        private string BuildDefaultInitScript()
        {
            var cols = Columns;

            var tableBuilder = new StringBuilder()
                                .Append($"IF OBJECT_ID(N'[{TableName}]', N'U') IS NULL ")
                                .Append("BEGIN ")
                                .Append($"CREATE TABLE [{TableName}](");

            for (int i = 0; i < cols.Length; i++)
            {
                var col = cols[i];

                tableBuilder.Append($"[{col.Name}] {col.TypeDefinition}{(i < cols.Length - 1 ? "," : "")}");
            }

            tableBuilder = tableBuilder.Append(")")
                                       .Append(" END");

            return tableBuilder.ToString();
        }

        private static LogTableColumnSchema[] GetDefaultColumns()
        {
            return new LogTableColumnSchema[]
            {
                IdColumn,
                TimestampColumn,
                MessageColumn,
                ExceptionColumn,
                LevelColumn,
            };
        }
    }
}