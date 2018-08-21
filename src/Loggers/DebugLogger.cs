namespace NWrath.Logging
{
    public class DebugLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer { get; set; }

        public DebugLogger()
        {
            Serializer = new StringLogSerializer();

            System.Diagnostics.Debug.AutoFlush = true;
        }

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log);

            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}