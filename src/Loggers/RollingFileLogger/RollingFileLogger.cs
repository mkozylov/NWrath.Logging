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
        public Lazy<IFileLogger> Writer
        {
            get => _writer;

            set
            {
                if (_writer?.IsValueCreated ?? false)
                {
                    _writer.Value.Dispose();
                    _writer = value;
                }
            }
        }

        public PipeCollection<RollingFileContext> Pipes { get; set; }

        public IStringLogSerializer Serializer { get; set; }

        public IRollingFileProvider FileProvider { get; set; }

        public Encoding Encoding { get; set; }

        public static IPipe<RollingFileContext> LogWriterPipe = new LambdaPipe<RollingFileContext>(
           (ctx, next) =>
           {
               next(ctx);

               if (ctx.Logger.IsEnabled)
               {
                   ctx.Logger.Writer?.Value?.Log(ctx.LogMessage);
               }
           }
        );

        private Lazy<IFileLogger> _writer;

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
            Serializer = new StringLogSerializer();
            Encoding = Encoding.UTF8;

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
            _writer?.Value?.Dispose();

            _writer = null;
        }

        protected override void WriteLog(LogMessage log)
        {
            var ctx = ProduceContext(log);

            var pipes = Pipes;

            if (pipes?.Count > 0)
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