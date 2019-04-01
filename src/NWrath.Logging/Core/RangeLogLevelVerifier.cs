namespace NWrath.Logging
{
    public class RangeLogLevelVerifier
        : ILogRecordVerifier
    {
        public LogLevel MinLevel
        {
            get => _minLevel;

            set
            {
                if ((int)value > (int)_maxLevel)
                {
                    throw Errors.WRONG_LOG_LEVELS;
                }

                _minLevel = value;
            }
        }

        public LogLevel MaxLevel
        {
            get => _maxLevel;

            set
            {
                if ((int)value < (int)_minLevel)
                {
                    throw Errors.WRONG_LOG_LEVELS;
                }

                _maxLevel = value;
            }
        }

        private LogLevel _minLevel;
        private LogLevel _maxLevel;

        public RangeLogLevelVerifier(LogLevel minLevel, LogLevel maxLevel)
        {
            MaxLevel = maxLevel;
            MinLevel = minLevel;
        }

        public bool Verify(LogRecord record)
        {
            return record.Level >= _minLevel && record.Level <= _maxLevel;
        }
    }
}