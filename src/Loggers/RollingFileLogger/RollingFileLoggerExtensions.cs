using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

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
            source.Pipes.AddRange(collection);

            return source;
        }

        public static RollingFileLogger UsePipes(
            this RollingFileLogger source,
            params Action<RollingFileContext, Action<RollingFileContext>>[] collection
            )
        {
            source.Pipes.AddRange(collection);

            return source;
        }

        public static RollingFileLogger UseDefaultWriterPipe(this RollingFileLogger source)
        {
            source.Pipes.Add(RollingFileLogger.LogWriterPipe);

            return source;
        }

        public static RollingFileLogger UseDailyRollerPipe(
            this RollingFileLogger source,
            IRollingFileAction[] actions = null,
            bool instantNext = true
            )
        {
            actions = actions ?? new IRollingFileAction[] {
                 new RollingFileCreateFileAction(append: true, tryUseTodayLastFile: true),
                 new RollingFileCleanerAction(expirationTimeSpan: TimeSpan.FromDays(30))
            };

            source.Pipes.Add(
                new RollingFileDailyTrigger(actions, instantNext)
                );

            return source;
        }

        public static RollingFileLogger UseBytesLimitRollerPipe(
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