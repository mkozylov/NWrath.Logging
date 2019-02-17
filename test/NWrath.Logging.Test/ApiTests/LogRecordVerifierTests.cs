using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using System;
using System.Linq;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LogRecordVerifierTests
    {
        [Test]
        public void LogLevelVerifier_SetMinimumLevelToDebug()
        {
            AssertMinimumLevel(
                LogLevel.Debug,
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMinimumLevelToInfo()
        {
            AssertMinimumLevel(
                LogLevel.Info,
                new[] { LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical },
                new[] { LogLevel.Debug }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMinimumLevelToWarning()
        {
            AssertMinimumLevel(
                LogLevel.Warning,
                new[] { LogLevel.Warning, LogLevel.Error, LogLevel.Critical },
                new[] { LogLevel.Debug, LogLevel.Info }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMinimumLevelToError()
        {
            AssertMinimumLevel(
                LogLevel.Error,
                new[] { LogLevel.Error, LogLevel.Critical },
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMinimumLevelToCritical()
        {
            AssertMinimumLevel(
                LogLevel.Critical,
                new[] { LogLevel.Critical },
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMultipleLevelToAll()
        {
            AssertMultipleLevel(
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMultipleLevelToDebugWarningCritical()
        {
            AssertMultipleLevel(
                new[] { LogLevel.Debug, LogLevel.Warning, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMultipleLevelToInfoError()
        {
            AssertMultipleLevel(
                new[] { LogLevel.Info, LogLevel.Error }
                );
        }

        [Test]
        public void LogLevelVerifier_SetMultipleLevelToEmpty_Throw()
        {
            Assert.Catch<ArgumentException>(
                () => new MultipleLogLevelVerifier(new LogLevel[] { })
                );

            Assert.Catch<ArgumentException>(
                () => new MultipleLogLevelVerifier(new LogLevel[] { LogLevel.Debug }) { Levels = new LogLevel[] { } }
                );
        }

        [Test]
        public void LogLevelVerifier_SetRangeLevelToDebugCritical()
        {
            AssertRangeLevel(
                LogLevel.Debug,
                LogLevel.Critical,
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetRangeLevelToWarningError()
        {
            AssertRangeLevel(
                LogLevel.Warning,
                LogLevel.Error,
                new[] { LogLevel.Warning, LogLevel.Error },
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetRangeLevelToErrorError()
        {
            AssertRangeLevel(
                LogLevel.Error,
                LogLevel.Error,
                new[] { LogLevel.Error },
                new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Critical }
                );
        }

        [Test]
        public void LogLevelVerifier_SetRangeLevelToErrorDebug_Throw()
        {
            Assert.Catch<ArgumentException>(
                 () => new RangeLogLevelVerifier(LogLevel.Error, LogLevel.Debug)
                 );

            Assert.Catch<ArgumentException>(
                () => new RangeLogLevelVerifier(LogLevel.Debug, LogLevel.Info) { MinLevel = LogLevel.Error, MaxLevel = LogLevel.Debug }
                );
        }

        #region Internal

        private void AssertMultipleLevel(LogLevel[] levels)
        {
            var verifier = new MultipleLogLevelVerifier(levels);

            var all = new[] { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical };

            levels.Each(x => Assert.That(verifier.Verify(new LogRecord { Level = x })));

            all.Except(levels)
               .Each(x => Assert.That(!verifier.Verify(new LogRecord { Level = x })));
        }

        private void AssertRangeLevel(LogLevel minLevel, LogLevel maxLevel, LogLevel[] positive, LogLevel[] negative = null)
        {
            LevelGeneralAssert(
                new RangeLogLevelVerifier(minLevel, maxLevel),
                positive,
                negative
                );
        }

        private void AssertMinimumLevel(LogLevel minLevel, LogLevel[] positive, LogLevel[] negative = null)
        {
            LevelGeneralAssert(
                new MinimumLogLevelVerifier(minLevel),
                positive,
                negative
                );
        }

        private void LevelGeneralAssert(ILogRecordVerifier verifier, LogLevel[] positive, LogLevel[] negative = null)
        {
            positive.Each(x => Assert.That(verifier.Verify(new LogRecord { Level = x })));

            negative?.Each(x => Assert.That(!verifier.Verify(new LogRecord { Level = x })));
        }

        #endregion Internal
    }
}