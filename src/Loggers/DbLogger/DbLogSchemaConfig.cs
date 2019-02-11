namespace NWrath.Logging
{
    public class DbLogSchemaConfig
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public string InitScript { get; set; }

        public LogTableColumnSchema[] Columns { get; set; }
    }
}