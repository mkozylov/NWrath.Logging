using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public abstract class DbLoggerBase
         : LoggerBase
    {
        protected IDbLogSchema schema;

        public DbLoggerBase(IDbLogSchema schema)
        {
            this.schema = schema;

            Init();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            var verifiedBatch = batch.Where(r => RecordVerifier.Verify(r))
                                     .ToArray();

            if (verifiedBatch.Length == 0)
            {
                return;
            }

            using (var con = schema.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = schema.BuildInsertBatchQuery(verifiedBatch);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            using (var con = schema.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = schema.BuildInsertQuery(record);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected virtual void ExecuteInitScript()
        {
            using (var con = schema.CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = schema.InitScript;
                cmd.CommandType = System.Data.CommandType.Text;

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected virtual void Init()
        {
            if (!string.IsNullOrEmpty(schema.InitScript))
            {
                ExecuteInitScript();
            }
        }
    }
}