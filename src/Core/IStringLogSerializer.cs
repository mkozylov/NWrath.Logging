namespace NWrath.Logging
{
    public interface IStringLogSerializer
    {
        string Serialize(LogRecord record);
    }
}