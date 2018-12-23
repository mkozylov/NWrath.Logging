namespace NWrath.Logging
{
    public interface IRollingFileLoggerInternal
    {
        IFileLogger Writer { get; }
    }
}