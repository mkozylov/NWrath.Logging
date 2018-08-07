using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Pipeline;
using NWrath.Synergy.Common.Extensions;
using System.Threading.Tasks;
using System.Collections;
using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public class PipeLogger<TLogger>
         : LoggerBase
        where TLogger : ILogger
    {
        public static IPipe<PipeLoggerContext<TLogger>> LogWriterPipe = new LambdaPipe<PipeLoggerContext<TLogger>>(
            (ctx, next) =>
            {
                next(ctx);
                ctx.Logger?.Log(ctx.LogMessage);
            }
        );

        public PipeCollection<PipeLoggerContext<TLogger>> Pipes { get; set; }

        private TLogger _logger;

        public PipeLogger(
            TLogger logger,
            PipeCollection<PipeLoggerContext<TLogger>> pipes = null
            )
        {
            _logger = logger;

            Pipes = pipes ?? ProduceDefaultPipes();
        }

        protected override void WriteLog(LogMessage log)
        {
            var pipes = Pipes;

            var ctx = ProduceContext(log);

            if (pipes?.Count > 0)
            {
                pipes.Pipeline.Perform(ctx);
            }
        }

        private PipeCollection<PipeLoggerContext<TLogger>> ProduceDefaultPipes()
        {
            return new PipeCollection<PipeLoggerContext<TLogger>>
            {
                (ctx, next) => {
                    next(ctx);
                    _logger.Log(ctx.LogMessage);
                    }
            };
        }

        private PipeLoggerContext<TLogger> ProduceContext(LogMessage log)
        {
            return new PipeLoggerContext<TLogger>(_logger, log);
        }
    }
}