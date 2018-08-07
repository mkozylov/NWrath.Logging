using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class MinimumLogLevelVerifier
        : ILogLevelVerifier
    {
        public virtual LogLevel MinimumLevel { get; private set; } = LogLevel.Debug;

        public MinimumLogLevelVerifier(LogLevel minLevel)
        {
            SetMinimumLevel(minLevel);
        }

        public virtual void SetMinimumLevel(LogLevel minLevel)
        {
            MinimumLevel = minLevel;
        }

        public virtual bool Verify(LogLevel level)
        {
            return level >= MinimumLevel;
        }
    }
}