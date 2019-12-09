using System.Linq;
using System.Text;
using NWrath.Synergy.Pipeline;

namespace NWrath.Logging
{
    public class RollingFileWriterPipe
        : PipeBase<RollingFileContext>
    {
        public override void Perform(RollingFileContext context)
        {
            PerformNext(context);

            if (context.IsLoggerEnabled)
            {
                if (!context.FileProvider.Directory.Exists)
                {
                    context.FileProvider.Directory.Create();
                    context.LogFile.Change(context.LogFile.Path, true);
                }

                context.LogFile.Write(context.Batch);
                context.LogFile.Flush();
            }
        }
    }
}