using Newtonsoft.Json;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class LogTableSchema
        : ILogTableSchema
    {
        #region DefaultColumns

        public static LogTableColumnSchema IdColumn = new LogTableColumnSchema
        {
            Name = "Id",
            RawDbType = "BIGINT",
            IsKey = true,
            AllowNull = false,
            IsInternal = true
        };

        public static LogTableColumnSchema TimestampColumn = new LogTableColumnSchema
        {
            Name = "Timestamp",
            RawDbType = "DATETIME",
            AllowNull = false,
            Serializer = new LambdaLogSerializer(m => $"{m.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}")
        };

        public static LogTableColumnSchema MessageColumn = new LogTableColumnSchema
        {
            Name = "Message",
            RawDbType = "VARCHAR(MAX)",
            AllowNull = false,
            Serializer = new LambdaLogSerializer(m => $"{m.Message}")
        };

        public static LogTableColumnSchema ExceptionColumn = new LogTableColumnSchema
        {
            Name = "Exception",
            RawDbType = "VARCHAR(MAX)",
            AllowNull = true,
            Serializer = new LambdaLogSerializer(m => $"{m.Exception}")
        };

        public static LogTableColumnSchema LevelColumn = new LogTableColumnSchema
        {
            Name = "Level",
            RawDbType = "INT",
            AllowNull = false,
            Serializer = new LambdaLogSerializer(m => m.Level)
        };

        public static LogTableColumnSchema ExtraColumn = new LogTableColumnSchema
        {
            Name = "Extra",
            RawDbType = "VARCHAR(MAX)",
            AllowNull = true,
            Serializer = new LambdaLogSerializer(m => (m.Extra.Count == 0 ? null : m.Extra.AsJson()))
        };

        #endregion DefaultColumns

        public const string DefaultTableName = "ServerLog";

        public virtual string TableName { get; set; }

        public virtual string InserLogScript { get; set; }

        public virtual string InitScript { get; set; }

        protected List<LogTableColumnSchema> columns = new List<LogTableColumnSchema>();

        public LogTableSchema(
            string tableName = DefaultTableName,
            string initScript = null,
            string inserLogScript = null,
            LogTableColumnSchema[] columns = null
            )
        {
            TableName = tableName;
            InitScript = initScript;
            InserLogScript = inserLogScript;

            if (columns?.Length > 0)
            {
                this.columns.Clear();
                this.columns.AddRange(columns);
            }
        }

        public virtual void Build()
        {
            TableName = TableName ?? DefaultTableName;

            columns = (columns.IsEmpty()
                        ? GetDefaultColumns().ToList()
                        : columns);

            InserLogScript = InserLogScript ?? BuildDefaultInserLogScript();

            InitScript = InitScript ?? BuildDefaultInitScript();
        }

        public virtual LogTableSchema ApplyColumn(string columnName, Action<LogTableColumnSchema> apply)
        {
            var column = GetColumn(columnName);

            if (column != null)
            {
                apply(column);
            }

            return this;
        }

        public virtual LogTableSchema ClearColumns()
        {
            columns.Clear();

            return this;
        }

        public virtual LogTableSchema AddColumn(params LogTableColumnSchema[] columns)
        {
            this.columns.AddRange(columns);

            return this;
        }

        public virtual LogTableSchema AddColumn(Action<LogTableColumnSchema> columnApply)
        {
            var column = new LogTableColumnSchema();

            columnApply(column);

            columns.Add(column);

            return this;
        }

        public virtual LogTableColumnSchema[] GetColumns()
        {
            return columns.ToArray();
        }

        public virtual LogTableColumnSchema GetColumn(string columnName)
        {
            return columns.FirstOrDefault(x => x.Name == columnName);
        }

        private string BuildDefaultInserLogScript()
        {
            var columnNames = columns.Where(x => !x.IsInternal)
                                     .Select(x => x.Name)
                                     .ToList();

            var columnsStr = string.Join(", ", columnNames);
            var valsStr = string.Join(", ", columnNames.Select(x => $"@{x}"));

            return $"INSERT INTO {TableName}({columnsStr}) VALUES({valsStr})";
        }

        private string BuildDefaultInitScript()
        {
            var cols = columns.OrderByDescending(c => c.IsKey)
                              .ToList();

            var tableBuilder = new StringBuilder()
                                .Append($"IF OBJECT_ID(N'{TableName}', N'U') IS NULL ")
                                .Append("BEGIN ")
                                .Append($"CREATE TABLE {TableName}(");

            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];

                tableBuilder = tableBuilder.Append($"{col.Name} {col.RawDbType}")
                                           .Append($"{ThenValElseEmpty(!col.AllowNull, " NOT")} NULL")
                                           .Append($"{ThenValElseEmpty(col.IsKey, " PRIMARY KEY IDENTITY")}")
                                           .Append($"{ThenValElseEmpty(i < cols.Count - 1, ",")}");
            }

            tableBuilder = tableBuilder.Append(")")
                                       .Append(" END");

            return tableBuilder.ToString();
        }

        private LogTableColumnSchema[] GetDefaultColumns()
        {
            var newMessageColumn = MessageColumn.Clone();
            newMessageColumn.Serializer = new LambdaLogSerializer(m => $"{m.Message}{(m.Exception == null ? "" : Environment.NewLine)}{m.Exception}");

            return new LogTableColumnSchema[]
            {
                IdColumn,
                TimestampColumn,
                newMessageColumn,
                LevelColumn,
            };
        }

        private static string ThenValElseEmpty(bool predicate, string thenVal)
        {
            return predicate ? thenVal : "";
        }
    }
}