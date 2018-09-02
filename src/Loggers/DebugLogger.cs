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

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log);

            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}