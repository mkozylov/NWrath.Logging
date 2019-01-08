namespace NWrath.Logging
{
    public interface ILogTableSchema
    {
        string InitScript { get; }

        string TableName { get; }

        LogTableColumnSchema[] Columns { get; }

        string BuildInsertQuery(LogRecord record);

        string BuildInsertBatchQuery(LogRecord[] batch);
    }
}