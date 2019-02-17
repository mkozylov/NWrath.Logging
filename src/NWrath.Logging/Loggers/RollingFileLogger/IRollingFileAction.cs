namespace NWrath.Logging
{
    public interface IRollingFileAction
    {
        void Execute(RollingFileContext ctx);
    }
}