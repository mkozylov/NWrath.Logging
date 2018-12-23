using NWrath.Synergy.Pipeline;

namespace NWrath.Logging
{
    public class RollingFileBytesLimitTrigger
        : PipeBase<RollingFileContext>
    {
        public long BytesLimit { get; set; }

        private IRollingFileAction _actionTrigger;

        public RollingFileBytesLimitTrigger(
            IRollingFileAction actionTrigger,
            long bytesLimit = 1024 * 1024 * 1024
            )
        {
            BytesLimit = bytesLimit;

            _actionTrigger = actionTrigger;
        }

        public override void Perform(RollingFileContext context)
        {
            if (Predicate(context))
            {
                _actionTrigger.Execute(context);
            }

            PerformNext(context);
        }

        public bool Predicate(RollingFileContext ctx)
        {
            var str = ctx.Serializer.Serialize(ctx.LogRecord);

            var bytes = ctx.Encoding.GetBytes(str);

            return (ctx.LogFile.Size + bytes.Length) > BytesLimit;
        }
    }
}