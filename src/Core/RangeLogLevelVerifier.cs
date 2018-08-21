using NWrath.Synergy.Common.Extensions;
using System;

namespace NWrath.Logging
{
    public class RangeLogLevelVerifier
        : ILogLevelVerifier
    {
        public LogLevel[] Range { get; private set; }

        public RangeLogLevelVerifier(LogLevel minLevel, LogLevel maxLevel)
        {
            SetRangeLevel(minLevel, maxLevel);
        }

        public void SetRangeLevel(LogLevel minLevel, LogLevel maxLevel)
        {
            minLevel.Required(
                l => (int)l <= (int)maxLevel,
                () => throw new ArgumentException($"The min level({minLevel}) can not be higher than the max level({maxLevel})")
                );

            Range = new[] { minLevel, maxLevel };
        }

        public bool Verify(LogLevel level)
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