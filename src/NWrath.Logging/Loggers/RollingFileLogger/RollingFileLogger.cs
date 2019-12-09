using NWrath.Synergy.Common;
using NWrath.Synergy.Pipeline;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace NWrath.Logging
{
    public class RollingFileLogger
         : LoggerBase, IRollingFileLoggerInternal
    {
        FileLogger IRollingFileLoggerInternal.Writer => _writer;

        public PipeCollection<RollingFileContext> Pipes
        {
            get => _pipes;
            set => _pipes = value ?? new PipeCollection<RollingFileContext>(new[] { new RollingFileWriterPipe() });
        }

        public IStringLogSerializer Serializer
        {
            get => _writer.Serializer;
            set => _writer.Serializer = value ?? StringLogSerializerBuilder.DefaultSerializer;
        }

        public Encoding Encoding
        {
            get => _writer.Encoding;
            set => _writer.Encoding = value ?? new UTF8Encoding(false);
        }

        public IRollingFileProvider FileProvider
        {
            get => _fileProvider;
            set => _fileProvider = value ?? throw Errors.NO_FILE_PROVIDER;
        }

        private FileLogger _writer;
        private IRollingFileProvider _fileProvider;
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

            if (!FileProvider.Directory.Exists)
            {
                FileProvider.Directory.Create();
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
            _writer.Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
            {
                return;
            }

            var verifiedBatch = batch.Where(r => RecordVerifier.Verify(r))
                                     .ToArray();

            if (verifiedBatch.Length == 0)
            {
                return;
            }

            var ctx = ProduceContext(verifiedBatch);

            Pipes.Pipeline?.Perform(ctx);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord record)
        {
            if (IsEnabled && RecordVerifier.Verify(record))
            {
                WriteRecord(record);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var ctx = ProduceContext(new []{ record });

            Pipes.Pipeline?.Perform(ctx);
        }

        private void SetDefaultWriter()
        {
            var file = FileProvider.TryResolveLastFile();

            if (file == null
                || !file.Exists
                || file.CreationTime.Date != Clock.Today)
            {
                file = FileProvider.ProduceNewFile();
            }

            _writer = new FileLogger(file.FullName, append: true) { AutoFlush = false };
        }

        private void SetDefaultPipes()
        {
            this.AddDefaultWriterPipe();
            this.AddDailyRollerPipe();
        }

        private RollingFileContext ProduceContext(LogRecord[] batch)
        {
            return new RollingFileContext(this, _writer, batch);
        }
    }
}