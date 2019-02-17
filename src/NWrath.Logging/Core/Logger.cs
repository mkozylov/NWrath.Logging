using NWrath.Synergy.Common.Extensions;
using System;

namespace NWrath.Logging
{
    public static class Logger
    {
        public static ILogger Instance
        {
            get => _isInstanceSet ? _instance.Value : null;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _isInstanceSet = true;

                _instance = new Lazy<ILogger>(() => value);
            }
        }

        private static Lazy<ILogger> _instance = new Lazy<ILogger>(() => throw Errors.NO_LOGGERS);

        private static bool _isInstanceSet;

        public static void Log(LogRecord record)
        {
            _instance.Value.Log(record);
        }

        public static void Debug(string msg)
        {
            _instance.Value.Debug(msg);
        }

        public static void Info(string msg)
        {
            _instance.Value.Info(msg);
        }

        public static void Warning(string msg, Exception exception = null)
        {
            _instance.Value.Warning(msg, exception);
        }

        public static void Error(string msg, Exception exception = null)
        {
            _instance.Value.Error(msg, exception);
        }

        public static void Critical(string msg, Exception exception = null)
        {
            _instance.Value.Critical(msg, exception);
        }
    }
}