using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class MultipleLogLevelVerifier
        : ILogLevelVerifier
    {
        public virtual LogLevel[] Levels { get; private set; }

        public MultipleLogLevelVerifier(LogLevel[] levels)
        {
            SetMultipleLevel(levels);
        }

        public virtual void SetMultipleLevel(params LogLevel[] levels)
        {
            levels.Required(
                x => x.NotEmpty(),
                () => throw new ArgumentException("You must specify at least one log level")
                );

            Levels = levels;
        }

        public virtual bool Verify(LogLevel level)
        {
            return Levels.Contains(level);
        }
    }
}