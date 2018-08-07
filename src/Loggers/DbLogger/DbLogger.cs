using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using NWrath.Synergy.Common.Structs;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;
using NWrath.Synergy.Common.Extensions;
using System.Data.Common;

namespace NWrath.Logging
{
    public class DbLogger
         : LoggerBase
    {
        public string ConnectionString { get; set; }

        public ILogTableSchema TableSchema
        {
            get => _tableSchema;

            set
            {
                _tableSchema = value.Required(() => throw new ArgumentNullException(nameof(value)));

                Init();
            }
        }

        private Lazy<DbLogger> _self;
        private List<LogTableColumnSchema> _writeColumns;
        private ILogTableSchema _tableSchema = CreateDefaultLogTableSchema();

        public DbLogger(string connectionString, ILogTableSchema logTableSchema = null)
        {
            ConnectionString = connectionString;

            if (logTableSchema != null)
            {
                TableSchema = logTableSchema;
            }

            Init();
        }

        protected override void WriteLog(LogMessage log)
        {
            using (var con = CreateConnection(_self.Value.ConnectionString))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = TableSchema.InserLogScript;
                cmd.CommandType = GetCommandType(TableSchema.InserLogScript);

                con.Open();

                foreach (var col in _writeColumns)
                {
                    var p = cmd.CreateParameter();

                    p.ParameterName = $"@{col.Name}";

                    p.Value = col.Serializer.Serialize(log);

                    cmd.Parameters.Add(p);
                }

                cmd.ExecuteNonQuery();
            }
        }

        protected virtual DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        protected virtual void ExecuteInitScript(string connectionString)
        {
            using (var con = CreateConnection(connectionString))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = TableSchema.InitScript;
                cmd.CommandType = GetCommandType(TableSchema.InitScript);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private void Init()
        {
            _self = new Lazy<DbLogger>(() =>
            {
                if (!string.IsNullOrEmpty(TableSchema.InitScript))
                {
                    ExecuteInitScript(ConnectionString);
                }

                _writeColumns = TableSchema.GetColumns()
                                           .Where(x => !x.IsInternal)
                                           .ToList();

                if (_writeColumns.Count == 0)
                {
                    throw new Exception($"There is no columns for write");
                }

                return this;
            });
        }

        private static ILogTableSchema CreateDefaultLogTableSchema()
        {
            var schema = new LogTableSchema();

            schema.Build();

            return schema;
        }

        private System.Data.CommandType GetCommandType(string cmdText)
        {
            return cmdText.Contains(' ')
                ? System.Data.CommandType.Text
                : System.Data.CommandType.StoredProcedure;
        }
    }
}