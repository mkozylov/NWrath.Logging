namespace NWrath.Logging
{
    public interface ILogSerializer
    {
        object Serialize(LogMessage log);
    }
}