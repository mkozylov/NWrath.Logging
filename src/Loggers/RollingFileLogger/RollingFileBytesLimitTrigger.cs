using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public virtual bool Predicate(RollingFileContext ctx)
        {
            var str = ctx.Logger.Serializer.Serialize(ctx.LogMessage);

            var bytes = ctx.Logger.Encoding.GetBytes(str);

            return (ctx.Logger.Writer.Value.FileSize + bytes.Length) > BytesLimit;
        }
    }
}