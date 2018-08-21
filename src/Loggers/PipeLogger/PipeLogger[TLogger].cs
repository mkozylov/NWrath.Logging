using NWrath.Synergy.Pipeline;

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

        public override bool IsEnabled
        {
            get => _logger.IsEnabled;
            set => _logger.IsEnabled = value;
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => _logger.LevelVerifier;
            set => _logger.LevelVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug);
        }

        public PipeCollection<PipeLoggerContext<TLogger>> Pipes { get; set; }

        private TLogger _logger;

        public PipeLogger(
            TLogger logger
            )
        {
            _logger = logger;

            Pipes = ProduceDefaultPipes();
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