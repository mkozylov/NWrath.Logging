using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Pipeline;
using System;

namespace NWrath.Logging
{
    public class PipeLogger<TLogger>
         : LoggerBase
        where TLogger : class, ILogger
    {
        public static IPipe<PipeLoggerContext<TLogger>> LogWriterPipe => new LambdaPipe<PipeLoggerContext<TLogger>>(
            (ctx, next) =>
            {
                next(ctx);
                ctx.Logger?.Log(ctx.LogRecord);
            }
        );

        public TLogger BaseLogger
        {
            get => _baseLogger;
            set { _baseLogger = value ?? throw Errors.NULL_BASE_LOGGER; }
        }

        public PipeCollection<PipeLoggerContext<TLogger>> Pipes
        {
            get => _pipes;

            set { _pipes = value ?? new PipeCollection<PipeLoggerContext<TLogger>> { LogWriterPipe }; }
        }

        public Set Properties { get => _properties; set { _properties = value ?? new Set(); } }

        private TLogger _baseLogger;
        private Set _properties = new Set();
        private PipeCollection<PipeLoggerContext<TLogger>> _pipes = new PipeCollection<PipeLoggerContext<TLogger>> { LogWriterPipe };
        private bool _leaveOpen;

        public PipeLogger(
            TLogger logger,
            bool leaveOpen = false
            )
        {
            BaseLogger = logger;
            _leaveOpen = leaveOpen;
        }

        ~PipeLogger()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (!_leaveOpen)
            {
                BaseLogger.Dispose();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var pipes = Pipes;

            var ctx = ProduceContext(record);

            if (pipes.Count > 0)
            {
                pipes.Pipeline.Perform(ctx);
            }
        }

        private PipeLoggerContext<TLogger> ProduceContext(LogRecord record)
        {
            return new PipeLoggerContext<TLogger>(_baseLogger, record, Properties);
        }
    }
}