namespace NWrath.Logging
{
    public class EmptyLogger
         : LoggerBase
    {
        public override void Log(LogRecord record)
        {
        }

        protected override void WriteRecord(LogRecord record)
        {
        }
    }
}