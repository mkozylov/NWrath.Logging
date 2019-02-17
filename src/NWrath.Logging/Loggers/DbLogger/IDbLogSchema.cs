using System.Data.Common;

namespace NWrath.Logging
{
    public interface IDbLogSchema
    {
        string ConnectionString { get; set; }

        string InitScript { get; }

        string TableName { get; }

        LogTableColumnSchema[] Columns { get; }

        string BuildInsertQuery(LogRecord record);

        string BuildInsertBatchQuery(LogRecord[] batch);

        DbConnection CreateConnection();
    }
}