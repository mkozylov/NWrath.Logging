using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NWrath.Logging
{
    public class BackgroundLogger
         : LoggerBase
    {
        public static readonly int DefaultBatchSize = 100;

        public static readonly TimeSpan DefaultFlushPeriod = TimeSpan.FromSeconds(1);

        public override bool IsEnabled
        {
            get => base.IsEnabled;

            set
            {
                if (base.IsEnabled == value) return;

                base.IsEnabled = value;

                if (value)
                {
                    InitQueue();
                }
                else
                {
                    Dispose();
                }
            }
        }

        public override ILogLevelVerifier LevelVerifier
        {
            get => BaseLogger.LevelVerifier;
            set { BaseLogger.LevelVerifier = value ?? new MinimumLogLevelVerifier(LogLevel.Debug); }
        }

        public ILogger BaseLogger
        {
            get => _baseLogger;
            set { _baseLogger = value ?? throw new ArgumentNullException(Errors.NULL_BASE_LOGGER); }
        }

        public TimeSpan FlushPeriod { get; private set; }

        public int BatchSize { get; private set; }

        private ILogger _baseLogger;
        private Task _watchTask;
        private Lazy<BatchBlock<LogRecord>> _queue;
        private ActionBlock<LogRecord[]> _writeBlock;
        private CancellationTokenSource _cts;
        private bool _leaveOpen;

        public BackgroundLogger(
            ILogger baseLogger,
            TimeSpan? flushPeriod = null,
            int? batchSize = null,
            bool leaveOpen = false
            )
        {
            BaseLogger = baseLogger;
            FlushPeriod = flushPeriod ?? TimeSpan.FromSeconds(1);
            BatchSize = batchSize ?? DefaultBatchSize;
            _leaveOpen = leaveOpen;

            InitQueue();
        }

        ~BackgroundLogger()
        {
            Dispose();
        }

        public override void Dispose()
        {
            _cts?.Cancel();
            _watchTask?.Wait();

            if (_queue.IsValueCreated)
            {
                _queue.Value.Complete();
                _writeBlock.Completion.Wait();
            }

            if (!_leaveOpen)
            {
                _baseLogger.Dispose();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            _queue.Value.Post(record);
        }

        private void StartWatchBuffer()
        {
            _cts = new CancellationTokenSource();

            _watchTask = Task.Run(WatchBufferLoopAsync);
        }

        private void InitQueue()
        {
            _queue = new Lazy<BatchBlock<LogRecord>>(() =>
            {
                var buffer = new BatchBlock<LogRecord>(
                    BatchSize,
                    new GroupingDataflowBlockOptions
                    {
                        EnsureOrdered = true
                    });

                _writeBlock = new ActionBlock<LogRecord[]>(
                    batch => _baseLogger.Log(batch),
                    new ExecutionDataflowBlockOptions
                    {
                        EnsureOrdered = true
                    });

                buffer.LinkTo(_writeBlock, new DataflowLinkOptions { PropagateCompletion = true });

                StartWatchBuffer();

                return buffer;
            });
        }

        private async Task WatchBufferLoopAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_queue.IsValueCreated)
                {
                    _queue.Value.TriggerBatch();
                }

                try
                {
                    await Task.Delay(FlushPeriod, _cts.Token)
                        .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    //ignore
                }
            }
        }
    }
}