namespace NWrath.Logging
{
    public interface IRollingFileLoggerInternal
    {
        FileLogger Writer { get; }
    }
}