using System.IO;

namespace NWrath.Logging
{
    public interface IRollingFileProvider
    {
        DirectoryInformation Directory { get; }

        FileInformation[] GetFiles();

        FileInformation ProduceNewFile();

        FileInformation TryResolveLastFile();
    }
}