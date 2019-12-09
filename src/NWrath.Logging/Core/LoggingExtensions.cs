using Newtonsoft.Json.Linq;
using NWrath.Synergy.Pipeline;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NWrath.Synergy.Common.Structs;
using System.IO;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public static class LoggingExtensions
    {
        public static void Log(
            this ILogger logger,
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            StringSet extra = null
        )
        {
            logger.Log(new LogRecord(message, timestamp, level, exception, extra));
        }

        public static void Log<TExtra>(
            this ILogger logger,
            string message,
            DateTime? timestamp = null,
            LogLevel level = LogLevel.Debug,
            Exception exception = null,
            TExtra extra = default(TExtra)
        )
        {
            logger.Log(new LogRecord(message, timestamp, level, exception, (extra as StringSet) ?? extra.ToStringSet()));
        }

        public static void Debug(this ILogger logger, string msg, StringSet extra = null)
        {
            logger.Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public static void Debug<TExtra>(this ILogger logger, string msg, TExtra extra = default(TExtra))
        {
            logger.Log(msg, level: LogLevel.Debug, extra: extra);
        }

        public static void Info(this ILogger logger, string msg, StringSet extra = null)
        {
            logger.Log(msg, level: LogLevel.Info, extra: extra);
        }

        public static void Info<TExtra>(this ILogger logger, string msg, TExtra extra = default(TExtra))
        {
            logger.Log(msg, level: LogLevel.Info, extra: extra);
        }

        public static void Warning(
            this ILogger logger, 
            string msg, 
            Exception exception = null, 
            StringSet extra = null
            )
        {
            logger.Log(msg, level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public static void Warning<TExtra>(
            this ILogger logger, 
            string msg, 
            Exception exception = null, 
            TExtra extra = default(TExtra)
            )
        {
            logger.Log(msg, level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public static void Warning(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public static void Warning<TExtra>(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Warning, exception: exception, extra: extra);
        }

        public static void Error(
            this ILogger logger, 
            string msg, 
            Exception exception = null, 
            StringSet extra = null
            )
        {
            logger.Log(msg, level: LogLevel.Error, exception: exception, extra: extra);
        }

        public static void Error<TExtra>(
            this ILogger logger, 
            string msg, 
            Exception exception = null, 
            TExtra extra = default(TExtra)
            )
        {
            logger.Log(msg, level: LogLevel.Error, exception: exception, extra: extra);
        }

        public static void Error(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Error, exception: exception, extra: extra);
        }

        public static void Error<TExtra>(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Error, exception: exception, extra: extra);
        }

        public static void Critical(
            this ILogger logger, string msg, 
            Exception exception = null, 
            StringSet extra = null
            )
        {
            logger.Log(msg, level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public static void Critical<TExtra>(
            this ILogger logger, 
            string msg, 
            Exception exception = null, 
            TExtra extra = default(TExtra)
            )
        {
            logger.Log(msg, level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public static void Critical(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            StringSet extra = null
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Critical, exception: exception, extra: extra);
        }

        public static void Critical<TExtra>(
            this ILogger logger,
            Exception exception,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            TExtra extra = default(TExtra)
        )
        {
            logger.Log(ExtractCaller(callerMemberName, callerFilePath), level: LogLevel.Critical, exception: exception, extra: extra);
        }

        #region private

        private static string ExtractCaller(string callerMemberName, string callerFilePath)
        {
            return $"{Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMemberName}";
        }

        #endregion
    }
}