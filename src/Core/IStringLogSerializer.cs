namespace NWrath.Logging
{
    public interface IStringLogSerializer
    {
        string Serialize(LogMessage log);
    }
}