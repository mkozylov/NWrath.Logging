using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Pipeline;
using System;

namespace NWrath.Logging
{
    public class PipeLogger<TLogger>
         : LoggerBase
        where TLogger : class, ILogger
    {
        public static IPipe<PipeLoggerContext<TLogger>> LogWriterPipe = new LambdaPipe<PipeLoggerContext<TLogger>>(
            (ctx, next) =>
            {
                next(ctx);
                ctx.Logger?.Log(ctx.LogRecord);
            }
        );

        public override bool IsEnabled
        {
            get => _baseLogger.IsEnabled;
            set => _baseLogger.IsEnabled = value;
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => _baseLogger.LevelVerifier;
            set => _baseLogger.LevelVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug);
        }

        public TLogger BaseLogger
        {
            get => _baseLogger;
            set { _baseLogger = value ?? throw new ArgumentNullException(Errors.NULL_BASE_LOGGER); }
        }

        public PipeCollection<PipeLoggerContext<TLogger>> Pipes
        {
            get => _pipes;

            set { _pipes = value ?? new PipeCollection<PipeLoggerContext<TLogger>>(); }
        }

        public Set Properties { get => _properties; set { _properties = value ?? new Set(); } }

        private TLogger _baseLogger;
        private Set _properties = new Set();
        private PipeCollection<PipeLoggerContext<TLogger>> _pipes = new PipeCollection<PipeLoggerContext<TLogger>>();
        private bool _leaveOpen;

        public PipeLogger(
            TLogger logger,
            bool leaveOpen = false
            )
        {
            BaseLogger = logger;
            _leaveOpen = leaveOpen;
            Pipes = ProduceDefaultPipes();
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
                    _baseLogger.Log(ctx.LogRecord);
                    }
            };
        }

        private PipeLoggerContext<TLogger> ProduceContext(LogRecord record)
        {
            return new PipeLoggerContext<TLogger>(_baseLogger, record, Properties);
        }
    }
}