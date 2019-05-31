using NWrath.Synergy.Common.Structs;
using System;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public interface ILogger
        : IDisposable
    {
        ILogRecordVerifier RecordVerifier { get; set; }

        bool IsEnabled { get; set; }

        void Log(LogRecord record);

        void Log(LogRecord[] batch);
    }
}