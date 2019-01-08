using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;

namespace NWrath.Logging
{
    public class DbLogger
         : LoggerBase
    {
        public string ConnectionString
        {
            get => _connectionString;

            set
            {
                _connectionString = value ?? throw Errors.NO_CONNECTION_STRING;
            }
        }

        public ILogTableSchema TableSchema
        {
            get => _tableSchema;

            set
            {
                _tableSchema = value ?? throw Errors.NULL_LOG_TABLE_SCHEMA;

                SelfInit();
            }
        }

        private Lazy<DbLogger> _self;
        private ILogTableSchema _tableSchema = new SqlLogTableSchema();
        private string _connectionString;

        public DbLogger(string connectionString)
        {
            ConnectionString = connectionString;

            SelfInit();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            using (var con = CreateConnection(_self.Value.ConnectionString))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = TableSchema.BuildInsertBatchQuery(batch);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            using (var con = CreateConnection(_self.Value.ConnectionString))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = TableSchema.BuildInsertQuery(record);

                con.Open();

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

        private void SelfInit()
        {
            _self = new Lazy<DbLogger>(() =>
            {
                if (!string.IsNullOrEmpty(TableSchema.InitScript))
                {
                    ExecuteInitScript(ConnectionString);
                }

                return this;
            });
        }

        private System.Data.CommandType GetCommandType(string cmdText)
        {
            return cmdText.Contains(' ')
                ? System.Data.CommandType.Text
                : System.Data.CommandType.StoredProcedure;
        }
    }
}