using NWrath.Synergy.Pipeline;
using System;

namespace NWrath.Logging
{
    public static class RollingFileLoggerExtensions
    {
        public static RollingFileLogger ClearPipes(this RollingFileLogger source)
        {
            source.Pipes.Clear();

            return source;
        }

        public static RollingFileLogger UsePipes(
            this RollingFileLogger source, params
            IPipe<RollingFileContext>[] collection
            )
        {
            source.Pipes.Clear();
            source.Pipes.AddRange(collection);

            return source;
        }

        public static RollingFileLogger UsePipes(
            this RollingFileLogger source,
            params Action<RollingFileContext, Action<RollingFileContext>>[] collection
            )
        {
            source.Pipes.Clear();
            source.Pipes.AddRange(collection);

            return source;
        }

        public static RollingFileLogger AddDefaultWriterPipe(this RollingFileLogger source)
        {
            source.Pipes.Add(RollingFileLogger.LogWriterPipe);

            return source;
        }

        public static RollingFileLogger AddDailyRollerPipe(
            this RollingFileLogger source,
            IRollingFileAction[] actions = null,
            bool instantNext = true
            )
        {
            actions = actions ?? new IRollingFileAction[] {
                 new RollingFileCreateFileAction(fileMode: System.IO.FileMode.Append, tryUseTodayLastFile: true),
                 new RollingFileCleanerAction(expirationTimeSpan: TimeSpan.FromDays(30))
            };

            source.Pipes.Add(
                new RollingFileDailyTrigger(actions, instantNext)
                );

            return source;
        }

        public static RollingFileLogger AddBytesLimitRollerPipe(
           this RollingFileLogger source,
           IRollingFileAction action = null,
           long bytesLimit = 1024 * 1024 * 1024
           )
        {
            action = action ?? new RollingFileDisableLoggerAction();

            source.Pipes.Add(
                new RollingFileBytesLimitTrigger(action, bytesLimit)
                );

            return source;
        }
    }
}