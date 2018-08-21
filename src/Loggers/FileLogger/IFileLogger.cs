using System;
using System.IO;

namespace NWrath.Logging
{
    public interface IFileLogger
        : ILogger, IDisposable
    {
        string FilePath { get; set; }

        long FileSize { get; }

        FileMode FileMode { get; set; }
    }
}