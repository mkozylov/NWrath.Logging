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
        private LogTableColumnSchema[] _writeColumns;
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
                cmd.CommandType = System.Data.CommandType.Text;

                con.Open();

                var sb = new StringBuilder();

                sb.Append("insert into [" + TableSchema.TableName + "](");

                foreach (var col in _writeColumns)
                {
                    sb.Append("[" + col.Name + "],");
                }

                sb.Replace(",", ") values", sb.Length - 1, 1);

                foreach (var record in batch)
                {
                    if (!VerifyRecord(record))
                    {
                        continue;
                    }

                    sb.Append("(");

                    foreach (var col in _writeColumns)
                    {
                        var val = col.Serializer.Serialize(record);

                        if (val is string)
                        {
                            sb.Append("'" + val + "',");
                        }
                        else if (val is Enum)
                        {
                            sb.Append((int)val + ",");
                        }
                        else if (val is bool)
                        {
                            sb.Append(Convert.ToInt32(val) + ",");
                        }
                        else
                        {
                            sb.Append(val + ",");
                        }
                    }

                    sb.Replace(",", "),", sb.Length - 1, 1);
                }

                sb.Replace(',', ';', sb.Length - 1, 1);

                cmd.CommandText = sb.ToString();

                cmd.ExecuteNonQuery();
            }
        }

        protected override void WriteRecord(LogRecord record)
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

                    p.ParameterName = col.Name;

                    p.Value = col.Serializer.Serialize(record) ?? DBNull.Value;

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

        private void SelfInit()
        {
            _self = new Lazy<DbLogger>(() =>
            {
                if (!string.IsNullOrEmpty(TableSchema.InitScript))
                {
                    ExecuteInitScript(ConnectionString);
                }

                _writeColumns = TableSchema.Columns
                                           .Where(x => !x.IsInternal)
                                           .ToArray();

                if (_writeColumns.Length == 0)
                {
                    throw new Exception($"There is no columns for write");
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