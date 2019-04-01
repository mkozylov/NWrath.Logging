using System.Linq;

namespace NWrath.Logging
{
    public class MultipleLogLevelVerifier
        : ILogRecordVerifier
    {
        public LogLevel[] Levels
        {
            get => _levels;

            set
            {
                if ((value?.Length ?? 0) == 0)
                {
                    throw Errors.NO_LOG_LEVELS;
                }

                _levels = value;
            }
        }

        private LogLevel[] _levels;

        public MultipleLogLevelVerifier(LogLevel[] levels)
        {
            Levels = levels;
        }

        public bool Verify(LogRecord record)
        {
            return Levels.Contains(record.Level);
        }
    }
}