using System;
using System.IO;

namespace NWrath.Logging
{
    public interface IFileLogger
        : ILogger
    {
        string FilePath { get; set; }

        long FileSize { get; }

        FileMode FileMode { get; set; }

        void Log(byte[] data);
    }
}