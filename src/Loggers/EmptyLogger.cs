namespace NWrath.Logging
{
    public class EmptyLogger
         : LoggerBase
    {
        public override void Log(LogRecord record)
        {
        }

        public override void Log(LogRecord[] batch)
        {
        }

        protected override void WriteRecord(LogRecord record)
        {
        }
    }
}