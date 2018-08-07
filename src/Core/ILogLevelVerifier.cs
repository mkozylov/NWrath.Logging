namespace NWrath.Logging
{
    public interface ILogLevelVerifier
    {
        bool Verify(LogLevel level);
    }
}