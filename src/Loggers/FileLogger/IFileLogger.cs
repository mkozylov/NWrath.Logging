﻿using System;

namespace NWrath.Logging
{
    public interface IFileLogger
        : ILogger, IDisposable
    {
        string FilePath { get; set; }

        long FileSize { get; }
    }
}