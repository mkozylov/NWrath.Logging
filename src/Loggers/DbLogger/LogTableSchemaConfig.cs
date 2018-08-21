namespace NWrath.Logging
{
    public class LogTableSchemaConfig
    {
        public string TableName { get; set; }

        public string InserLogScript { get; set; }

        public string InitScript { get; set; }

        public LogTableColumnSchema[] Columns { get; set; }
    }
}