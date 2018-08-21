namespace NWrath.Logging
{
    public interface IRollingFileProvider
    {
        string FolderPath { get; }

        string[] GetFiles();

        string ProduceNewFile();

        string TryResolveLastFile();
    }
}