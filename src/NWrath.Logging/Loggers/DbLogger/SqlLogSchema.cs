using FastExpressionCompiler;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NWrath.Logging
{
    public class SqlLogSchema
        : IDbLogSchema
    {
        #region DefaultColumns

        public static ISqlLogColumnSchema  IdColumn => new SqlLogColumnSchema<long>
        (
            name: "Id",
            typeDefinition: "BIGINT NOT NULL PRIMARY KEY IDENTITY",
            isInternal: true
        );

        public static ISqlLogColumnSchema  TimestampColumn => new SqlLogColumnSchema<DateTime>
        (
            name: "Timestamp",
            typeDefinition: "DATETIME NOT NULL",
            isInternal: false,
            serializer: m => m.Timestamp
        );

        public static ISqlLogColumnSchema  MessageColumn => new SqlLogColumnSchema<string>
        (
            name: "Message",
            typeDefinition: "VARCHAR(MAX) NOT NULL",
            isInternal: false,
            serializer: m => m.Message
        );

        public static ISqlLogColumnSchema  ExceptionColumn => new SqlLogColumnSchema<string>
        (
            name: "Exception",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            serializer: m => m.Exception?.ToString()
        );

        public static ISqlLogColumnSchema  LevelColumn => new SqlLogColumnSchema<int>
        (
            name: "Level",
            typeDefinition: "INT NOT NULL",
            isInternal: false,
            serializer: m => (int)m.Level
        );

        public static ISqlLogColumnSchema  ExtraColumn => new SqlLogColumnSchema<string>
        (
            name: "Extra",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            serializer: m => (m.Extra.Count == 0 ? null : m.Extra.AsJson())
        );

        #endregion DefaultColumns

        public string ConnectionString
        {
            get => _connectionString;

            set => _connectionString = value ?? throw Errors.NO_CONNECTION_STRING;
        }

        public const string DefaultTableName = "ServerLog";

        public static ISqlLogColumnSchema [] DefaultColumns => GetDefaultColumns();

        public string TableName { get; private set; }

        public string InitScript { get; private set; }

        public ISqlLogColumnSchema [] Columns { get; private set; }

        IDbLogColumnSchema[] IDbLogSchema.Columns => Columns;

        private string _connectionString;
        private string _insertLogQueryPrefix;
        private Func<LogRecord, string> _insertLogQueryBuilder;

        public SqlLogSchema(
            string connectionString,
            string tableName = DefaultTableName,
            string initScript = null,
            ISqlLogColumnSchema [] columns = null
            )
        {
            ConnectionString = connectionString;
            Columns = columns?.Length > 0 ? columns : DefaultColumns;
            TableName = tableName ?? DefaultTableName;
            InitScript = initScript ?? BuildDefaultInitScript();
            _insertLogQueryPrefix = BuildInsertQueryPrefix();
            _insertLogQueryBuilder = CreateInsertLogQueryBuilder();
        }

        public virtual DbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public string BuildInsertQuery(LogRecord record)
        {
            return _insertLogQueryPrefix + _insertLogQueryBuilder(record);
        }

        public string BuildInsertBatchQuery(LogRecord[] batch)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < batch.Length; i++)
            {
                if (i == 0 || i % 1000 == 0)
                {
                    sb.Append("; " + _insertLogQueryPrefix);
                }

                sb.Append(_insertLogQueryBuilder(batch[i]));
                
                if ((i + 1) < batch.Length && (i + 1) % 1000 != 0)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        private string BuildInsertQueryPrefix()
        {
            return $"INSERT INTO [{TableName}]({string.Join(", ", Columns.Where(x => !x.IsInternal).Select(x => "[" + x.Name + "]"))}) VALUES";
        }

        private Func<LogRecord, string> CreateInsertLogQueryBuilder()
        {
            var sqlExtType = typeof(SqlTypeConverter);

            var columns = Columns.Where(x => !x.IsInternal)
                                     .ToList();

            var arg = Expression.Parameter(typeof(LogRecord));

            var block = new List<Expression> {
                Expression.Constant("(")
            };

            var serializeMI = typeof(IStringLogSerializer).GetMethod(nameof(IStringLogSerializer.Serialize));

            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];

                var serializeCall = Expression.Call(
                    Expression.Constant(col.Serializer),
                    serializeMI,
                    arg
                    );

                block.Add(serializeCall);

                if ((i + 1) < columns.Count)
                {
                    block.Add(
                        Expression.Constant(",")
                        );
                }
            }

            block.Add(
                Expression.Constant(")")
                );

            var concatExpr = ExpressionsHelper.BuildStringConcat(block.ToArray());

            var lambda = Expression.Lambda<Func<LogRecord, string>>(concatExpr, arg);

            return lambda.CompileFast();
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

        private static ISqlLogColumnSchema [] GetDefaultColumns()
        {
            return new ISqlLogColumnSchema []
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