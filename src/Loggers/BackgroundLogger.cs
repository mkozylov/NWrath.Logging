using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NWrath.Logging
{
    public class BackgroundLogger
         : LoggerBase, IDisposable
    {
        public override bool IsEnabled
        {
            get => base.IsEnabled;

            set
            {
                base.IsEnabled = value;

                if (value)
                {
                    StartWriterTask();
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

        private ILogger _baseLogger;
        private Task _writerTask;
        private ConcurrentQueue<LogMessage> _queue = new ConcurrentQueue<LogMessage>();
        private TimeSpan _flushPeriod;
        private CancellationTokenSource _cts;

        public BackgroundLogger(
            ILogger baseLogger,
            TimeSpan? flushPeriod = null
            )
        {
            BaseLogger = baseLogger;
            _flushPeriod = flushPeriod ?? TimeSpan.FromSeconds(1);

            StartWriterTask();
        }

        ~BackgroundLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            _cts.Cancel();

            _writerTask.Wait();
        }

        public override void Log(LogMessage log)
        {
            if (IsEnabled)
            {
                WriteLog(log);
            }
        }

        protected override void WriteLog(LogMessage log)
        {
            _queue.Enqueue(log);
        }

        private void StartWriterTask()
        {
            _cts = new CancellationTokenSource();

            _writerTask = Task.Run(WriterLoop, _cts.Token);
        }

        private async Task WriterLoop()
        {
            while (!_cts.IsCancellationRequested || _queue.Count > 0)
            {
                var count = _queue.Count;

                for (int i = 0; i < count; i++)
                {
                    if (_queue.TryDequeue(out LogMessage log))
                    {
                        _baseLogger.Log(log);
                    }
                }

                try
                {
                    await Task.Delay(_flushPeriod, _cts.Token)
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