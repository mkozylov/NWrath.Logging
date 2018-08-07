using System;
using System.Collections.Generic;
using System.Text;

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