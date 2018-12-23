namespace NWrath.Logging
{
    public class RollingFileDisableLoggerAction
        : IRollingFileAction
    {
        public void Execute(RollingFileContext ctx)
        {
            ctx.IsLoggerEnabled = false;
        }
    }
}