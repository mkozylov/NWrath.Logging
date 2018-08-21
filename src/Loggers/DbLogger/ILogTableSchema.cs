namespace NWrath.Logging
{
    public interface ILogTableSchema
    {
        string InitScript { get; }

        string InserLogScript { get; }

        string TableName { get; }

        LogTableColumnSchema[] Columns { get; }
    }
}