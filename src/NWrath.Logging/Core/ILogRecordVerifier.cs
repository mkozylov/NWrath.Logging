namespace NWrath.Logging
{
    public interface ILogRecordVerifier
    {
        bool Verify(LogRecord record);
    }
}