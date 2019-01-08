using System;
using System.IO;

namespace NWrath.Logging
{
    public interface IFileLogger
        : ILogger
    {
        string FilePath { get; }

        long FileSize { get; }

        void Log(byte[] data);

        void Flush();

        void SetFile(string filePath, bool append = true);
    }
}