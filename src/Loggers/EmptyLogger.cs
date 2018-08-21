namespace NWrath.Logging
{
    public class EmptyLogger
         : LoggerBase
    {
        public override void Log(LogMessage log)
        {
            WriteLog(log);
        }

        protected override void WriteLog(LogMessage log)
        {
        }
    }
}