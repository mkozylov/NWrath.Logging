using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Text;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Common;
using NWrath.Synergy.Pipeline;

namespace NWrath.Logging
{
    public class RollingFileLogger
         : LoggerBase
    {
        public static IPipe<RollingFileContext> LogWriterPipe => new LambdaPipe<RollingFileContext>(
            (ctx, next) =>
            {
                next(ctx);

                if (ctx.Logger.IsEnabled)
                {
                    ctx.Logger.Writer.Value.Log(ctx.LogRecord);
                }
            }
        );

        public Lazy<IFileLogger> Writer
        {
            get => _writer;

            set
            {
                Dispose();

                _writer = value;
            }
        }

        public PipeCollection<RollingFileContext> Pipes
        {
            get => _pipes;

            set { _pipes = value ?? new PipeCollection<RollingFileContext> { LogWriterPipe }; }
        }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? new UTF8Encoding(false); } }

        public IRollingFileProvider FileProvider { get => _fileProvider; set { _fileProvider = value ?? throw Errors.NO_FILE_PROVIDER; } }

        private Lazy<IFileLogger> _writer;
        private IRollingFileProvider _fileProvider;
        private IStringLogSerializer _serializer = new StringLogSerializer();
        private Encoding _encoding = new UTF8Encoding(false);
        private PipeCollection<RollingFileContext> _pipes = new PipeCollection<RollingFileContext> { LogWriterPipe };

        #region Ctor

        public RollingFileLogger(
            string folderPath
            )
            : this(
                new RollingFileProvider(folderPath)
            )
        {
        }

        public RollingFileLogger(
            IRollingFileProvider fileNameProvider
            )
        {
            FileProvider = fileNameProvider;

            if (!Directory.Exists(FileProvider.FolderPath))
            {
                Directory.CreateDirectory(FileProvider.FolderPath);
            }

            SetDefaultPipes();

            SetDefaultWriter();
        }

        ~RollingFileLogger()
        {
            Dispose();
        }

        #endregion Ctor

        public override void Dispose()
        {
            if (_writer.IsValueCreated)
            {
                _writer.Value.Dispose();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var ctx = ProduceContext(record);

            var pipes = Pipes;

            if (pipes.Count > 0)
            {
                pipes.Pipeline.Perform(ctx);
            }
        }

        private void SetDefaultWriter()
        {
            _writer = new Lazy<IFileLogger>(() =>
            {
                var fileName = FileProvider.TryResolveLastFile();

                if (fileName.IsEmpty()
                    || new FileInfo(fileName).CreationTime.Date != Clock.Today)
                {
                    fileName = FileProvider.ProduceNewFile();
                }

                return new FileLogger(fileName) { FileMode = FileMode.Append };
            });
        }

        private void SetDefaultPipes()
        {
            this.AddDailyRollerPipe();
        }

        private RollingFileContext ProduceContext(LogRecord record)
        {
            return new RollingFileContext(this, record);
        }
    }
}