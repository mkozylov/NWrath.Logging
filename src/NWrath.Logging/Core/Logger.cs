using NWrath.Synergy.Common.Structs;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public static class Logger
    {
        public static ILogger Instance
        {
            get => _instance;

            set
            {
                _instance = value ?? throw new ArgumentNullException();

                _lazyInstance = new Lazy<ILogger>(() => value);
            }
        }

        private static Lazy<ILogger> _lazyInstance = new Lazy<ILogger>(() => throw Errors.NO_LOGGERS);
        private static ILogger _instance;

        public static void Log(LogRecord record)
        {
            _lazyInstance.Value.Log(record);
        }

        public static void Log(LogRecord[] batch)
        {
            _lazyInstance.Value.Log(batch);
        }

        public static void Log(
           string message,
           DateTime? timestamp = null,
           LogLevel level = LogLevel.Debug,
           Exception exception = null,
           StringSet extra = null
       )
        {
            _lazyInstance.Value.Log(new LogRecord(message, timestamp, level, exception, extra));
        }

        public static void Log<TExtra>(
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            TExtra extra = default(TExtra)
        )
        {
            _lazyInstance.Value.Log(
                new LogRecord(
                    message, 
                    timestamp, 
                    level, 
                    exception, 
                    (extra as StringSet) ?? StringSet.FromObject(extra)
                    )
                );
        }

        public static void Debug(string msg, StringSet extra = null)
        {
            _lazyInstance.Value.Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public static void Debug<TExtra>(string msg, TExtra extra = default(TExtra))
        {
            _lazyInstance.Value.Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public static void Info(string msg, StringSet extra = null)
        {
            _lazyInstance.Value.Log(msg, level: LogLevel.Info, extra: extra);
        }

        public static void Info<TExtra>(string msg, TExtra extra = default(TExtra))
        {
            _lazyInstance.Value.Log(msg, level: LogLevel.Info, extra: extra);
        }

        public static void Warning(
            string msg,
            Exception exception = null,
            StringSet extra = null
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Warning, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Warning<TExtra>(
            string msg,
            Exception exception = null,
            TExtra extra = default(TExtra)
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Warning, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Warning(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Warning, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Warning<TExtra>(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Warning, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Error(
            string msg,
            Exception exception = null,
            StringSet extra = null
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Error, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Error<TExtra>(
            string msg,
            Exception exception = null,
            TExtra extra = default(TExtra)
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Error, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Error(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Error, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Error<TExtra>(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Error, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Critical(
            string msg,
            Exception exception = null,
            StringSet extra = null
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Critical, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Critical<TExtra>(
            string msg,
            Exception exception = null,
            TExtra extra = default(TExtra)
            )
        {
            _lazyInstance.Value.Log(
                msg, 
                level: LogLevel.Critical, 
                exception: exception, 
                extra: extra
                );
        }

        public static void Critical(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Critical, 
                exception: 
                exception, 
                extra: extra
                );
        }

        public static void Critical<TExtra>(
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            _lazyInstance.Value.Log(
                ExtractCaller(callerMemberName, callerFilePath), 
                level: LogLevel.Critical, 
                exception: exception, 
                extra: extra
                );
        }

        #region private

        private static string ExtractCaller(string callerMemberName, string callerFilePath)
        {
            return $"{Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMemberName}";
        }

        #endregion
    }
}