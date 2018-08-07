using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public class RangeLogLevelVerifier
        : ILogLevelVerifier
    {
        public virtual LogLevel[] Range { get; private set; }

        public RangeLogLevelVerifier(LogLevel minLevel, LogLevel maxLevel)
        {
            SetRangeLevel(minLevel, maxLevel);
        }

        public virtual void SetRangeLevel(LogLevel minLevel, LogLevel maxLevel)
        {
            minLevel.Required(
                l => (int)l <= (int)maxLevel,
                () => throw new ArgumentException($"The min level({minLevel}) can not be higher than the max level({maxLevel})")
                );

            Range = new[] { minLevel, maxLevel };
        }

        public virtual bool Verify(LogLevel level)
        {
            return level >= Range[0] && level <= Range[1];
        }

        private enum VerifierType
        {
            Minimum,
            Multiple,
            Range
        }
    }
}