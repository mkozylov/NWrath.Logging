using System;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public class DbLogger
         : LoggerBase
    {
        public IDbLogSchema Schema
        {
            get => _tableSchema;

            set
            {
                _tableSchema = value ?? throw Errors.NULL_LOG_TABLE_SCHEMA;

                SelfInit();
            }
        }

        private Lazy<DbLogger> _self;
        private IDbLogSchema _tableSchema;

        public DbLogger(IDbLogSchema schema)
        {
            Schema = schema;

            SelfInit();
        }

        public DbLogger(string connectionString)
            : this(new SqlLogSchema(connectionString))
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            var s = _self.Value.Schema;

            using (var con = s.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = s.BuildInsertBatchQuery(batch);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var s = _self.Value.Schema;

            using (var con = s.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = s.BuildInsertQuery(record);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected virtual void ExecuteInitScript()
        {
            using (var con = Schema.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = Schema.InitScript;
                cmd.CommandType = System.Data.CommandType.Text;

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private void SelfInit()
        {
            _self = new Lazy<DbLogger>(() =>
            {
                if (!string.IsNullOrEmpty(Schema.InitScript))
                {
                    ExecuteInitScript();
                }

                return this;
            });
        }
    }
}