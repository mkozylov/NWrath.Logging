namespace NWrath.Logging
{
    public class DebugLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer
        {
            get => _serializer; set => _serializer = value ?? StringLogSerializerBuilder.DefaultSerializer;
        }

        private IStringLogSerializer _serializer = StringLogSerializerBuilder.DefaultSerializer;

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