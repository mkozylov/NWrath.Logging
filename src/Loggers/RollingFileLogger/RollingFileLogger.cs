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
         : LoggerBase, IDisposable
    {
        public static IPipe<RollingFileContext> LogWriterPipe = new LambdaPipe<RollingFileContext>(
            (ctx, next) =>
            {
                next(ctx);

                if (ctx.Logger.IsEnabled)
                {
                    ctx.Logger.Writer.Value.Log(ctx.LogMessage);
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

            set { _pipes = value ?? new PipeCollection<RollingFileContext>().Add(LogWriterPipe); }
        }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? Encoding.UTF8; } }

        public IRollingFileProvider FileProvider { get => _fileProvider; set { _fileProvider = value ?? throw new ArgumentNullException(Errors.NO_FILE_PROVIDER); } }

        private Lazy<IFileLogger> _writer;
        private IRollingFileProvider _fileProvider;
        private IStringLogSerializer _serializer = new StringLogSerializer();
        private Encoding _encoding = Encoding.UTF8;
        private PipeCollection<RollingFileContext> _pipes = new PipeCollection<RollingFileContext>();

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

            SetDefaultPipes();

            _writer = new Lazy<IFileLogger>(CreateDefaultFileWriter);
        }

        ~RollingFileLogger()
        {
            Dispose();
        }

        #endregion Ctor

        public void Dispose()
        {
            if (_writer.IsValueCreated)
            {
                _writer.Value.Dispose();
            }
        }

        protected override void WriteLog(LogMessage log)
        {
            var ctx = ProduceContext(log);

            var pipes = Pipes;

            if (pipes.Count > 0)
            {
                pipes.Pipeline.Perform(ctx);
            }
        }

        private IFileLogger CreateDefaultFileWriter()
        {
            if (!Directory.Exists(FileProvider.FolderPath))
            {
                Directory.CreateDirectory(FileProvider.FolderPath);
            }

            var fileName = FileProvider.TryResolveLastFile();

            if (fileName.IsEmpty()
                || new FileInfo(fileName).CreationTime.Date != Clock.Today)
            {
                fileName = FileProvider.ProduceNewFile();
            }

            return new FileLogger(fileName) { FileMode = FileMode.Append };
        }

        private void SetDefaultPipes()
        {
            Pipes = new PipeCollection<RollingFileContext>();

            this.AddDefaultWriterPipe()
                .AddDailyRollerPipe();
        }

        private RollingFileContext ProduceContext(LogMessage log)
        {
            return new RollingFileContext(this, log);
        }
    }
}