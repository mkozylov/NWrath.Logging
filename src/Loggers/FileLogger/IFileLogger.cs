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

        void SetFile(string filePath, FileMode fileMode = FileMode.Append);
    }
}