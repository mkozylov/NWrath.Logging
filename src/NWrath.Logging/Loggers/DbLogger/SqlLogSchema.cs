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

        public static LogTableColumnSchema IdColumn => new LogTableColumnSchema
        (
            name: "Id",
            typeDefinition: "BIGINT NOT NULL PRIMARY KEY IDENTITY",
            isInternal: true,
            type: typeof(long)
        );

        public static LogTableColumnSchema TimestampColumn => new LogTableColumnSchema
        (
            name: "Timestamp",
            typeDefinition: "DATETIME NOT NULL",
            isInternal: false,
            type: typeof(DateTime),
            serializer: new LambdaLogSerializer(m => m.Timestamp)
        );

        public static LogTableColumnSchema MessageColumn => new LogTableColumnSchema
        (
            name: "Message",
            typeDefinition: "VARCHAR(MAX) NOT NULL",
            isInternal: false,
            type: typeof(string),
            serializer: new LambdaLogSerializer(m => m.Message)
        );

        public static LogTableColumnSchema ExceptionColumn => new LogTableColumnSchema
        (
            name: "Exception",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            type: typeof(string),
            serializer: new LambdaLogSerializer(m => m.Exception?.ToString())
        );

        public static LogTableColumnSchema LevelColumn => new LogTableColumnSchema
        (
            name: "Level",
            typeDefinition: "INT NOT NULL",
            isInternal: false,
            type: typeof(int),
            serializer: new LambdaLogSerializer(m => m.Level)
        );

        public static LogTableColumnSchema ExtraColumn => new LogTableColumnSchema
        (
            name: "Extra",
            typeDefinition: "VARCHAR(MAX) NULL",
            isInternal: false,
            type: typeof(string),
            serializer: new LambdaLogSerializer(m => (m.Extra.Count == 0 ? null : m.Extra.AsJson()))
        );

        #endregion DefaultColumns

        public string ConnectionString
        {
            get => _connectionString;

            set => _connectionString = value ?? throw Errors.NO_CONNECTION_STRING;
        }

        public const string DefaultTableName = "ServerLog";

        public static LogTableColumnSchema[] DefaultColumns => GetDefaultColumns();

        public string TableName { get; private set; }

        public string InitScript { get; private set; }

        public LogTableColumnSchema[] Columns { get; private set; }

        private string _connectionString;
        private string _insertLogQueryPrefix;
        private Func<LogRecord, string> _insertLogQueryBuilder;

        public SqlLogSchema(
            string connectionString,
            string tableName = DefaultTableName,
            string initScript = null,
            LogTableColumnSchema[] columns = null
            )
        {
            ConnectionString = connectionString;
            Columns = columns?.Length > 0 ? columns : DefaultColumns;
            TableName = tableName ?? DefaultTableName;
            InitScript = initScript ?? BuildDefaultInitScript();
            _insertLogQueryPrefix = BuildInsertQueryPrefix();
            _insertLogQueryBuilder = CreateInsertLogQueryBuilder();
        }

        public SqlLogSchema(DbLogSchemaConfig config)
            : this(config.ConnectionString, config.TableName, config.InitScript, config.Columns)
        {
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
            var sb = _insertLogQueryPrefix.ToStringBuilder();

            for (int i = 0; i < batch.Length; i++)
            {
                sb.Append(_insertLogQueryBuilder(batch[i]));

                if ((i + 1) < batch.Length)
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
            var sqlExtType = typeof(SqlConverter);

            var columns = Columns.Where(x => !x.IsInternal)
                                     .ToList();

            var arg = Expression.Parameter(typeof(LogRecord));

            var block = new List<Expression> {
                Expression.Constant("(")
            };

            var serializeMI = typeof(ILogSerializer).GetMethod(nameof(ILogSerializer.Serialize));

            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];

                var serializeCall = Expression.Call(
                    Expression.Constant(col.Serializer),
                    serializeMI,
                    arg
                    );

                var toSqlStringMI = sqlExtType.GetMethod(nameof(SqlConverter.ToSqlString), new[] { col.Type });

                toSqlStringMI = toSqlStringMI ?? sqlExtType.GetStaticGenericMethod(nameof(SqlConverter.ToSqlString), 1, 1).MakeGenericMethod(col.Type);

                var val = Expression.Call(toSqlStringMI, Expression.Convert(serializeCall, col.Type));

                block.Add(val);

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