using System;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public class SqlLogger
         : DbLoggerBase
    {
        public SqlLogger(SqlLogSchema schema)
            : base(schema)
        {
        }

        public SqlLogger(
            string connectionString,
            string tableName = "ServerLog",
            string initScript = null,
            ISqlLogColumnSchema[] columns = null
            )
            : base(new SqlLogSchema(connectionString, tableName, initScript, columns))
        {
        }
    }
}