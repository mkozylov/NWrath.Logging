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
                    Dispose(true);
                }
            }
        }

        public ILogger BaseLogger
        {
            get => _baseLogger;
            set { _baseLogger = value ?? throw Errors.NULL_BASE_LOGGER; }
        }

        public ILogger EmergencyLogger { get; set; }

        public TimeSpan FlushPeriod { get; private set; }

        public int BatchSize { get; private set; }

        public Exception LastError { get; private set; }

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

        public override void Log(LogRecord record)
        {
            if (IsEnabled && RecordVerifier.Verify(record))
            {
                WriteRecord(record);
            }
        }

        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            foreach (var record in batch)
            {
                if (RecordVerifier.Verify(record))
                {
                    _queue.Value.Post(record);
                }
            }
        }

        public override void Dispose()
        {
            Dispose(false);
        }

        protected override void WriteRecord(LogRecord record)
        {
            _queue.Value.Post(record);
        }

        private void Dispose(bool leaveOpenBaseLoger)
        {
            if (_queue.IsValueCreated)
            {
                _cts.Cancel();
                _watchTask.Wait();

                _queue.Value.Complete();
                _writeBlock.Completion.Wait();
            }

            if (!leaveOpenBaseLoger && !_leaveOpen)
            {
                _baseLogger.Dispose();
            }
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
                    (Action<LogRecord[]>)BaseLoggerWriteAction,
                    new ExecutionDataflowBlockOptions
                    {
                        EnsureOrdered = true
                    });

                buffer.LinkTo(_writeBlock, new DataflowLinkOptions { PropagateCompletion = true });

                StartWatchBuffer();

                return buffer;
            });
        }

        private void BaseLoggerWriteAction(LogRecord[] batch)
        {
            try
            {
                _baseLogger.Log(batch);
            }
            catch (Exception ex)
            {
                LastError = ex;

                if (EmergencyLogger != null)
                {
                    EmergencyLogger.Log(batch);
                    EmergencyLogger.Error($"Base logger error", ex);
                }
            }
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