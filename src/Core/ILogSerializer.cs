namespace NWrath.Logging
{
    public interface ILogSerializer
    {
        object Serialize(LogRecord record);
    }
}