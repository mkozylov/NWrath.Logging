namespace NWrath.Logging
{
    public class SqlLogSchemaConfig
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public string InitScript { get; set; }

        public ISqlLogColumnSchema[] Columns { get; set; }
    }
}