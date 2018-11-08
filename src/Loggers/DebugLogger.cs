namespace NWrath.Logging
{
    public class DebugLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        private IStringLogSerializer _serializer = new StringLogSerializer();

        public DebugLogger()
        {
            System.Diagnostics.Debug.AutoFlush = true;
        }

        protected override void WriteRecord(LogRecord record)
        {
            var msg = Serializer.Serialize(record);

            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}