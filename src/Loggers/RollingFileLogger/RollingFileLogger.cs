using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Common;
using System.Text.RegularExpressions;
using NWrath.Synergy.Pipeline;
using System.Threading.Tasks;

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
            string folderPath,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
            : this(
                new RollingFileProvider(folderPath),
                serializer,
                encoding,
                pipes
            )
        {
        }

        public RollingFileLogger(
            IRollingFileProvider fileNameProvider,
            IStringLogSerializer serializer = null,
            Encoding encoding = null,
            PipeCollection<RollingFileContext> pipes = null
            )
        {
            FileProvider = fileNameProvider;
            Serializer = serializer ?? new StringLogSerializer();
            Encoding = encoding ?? Encoding.UTF8;
            Pipes = pipes ?? ProduceDefaultPipes();

            _writer = new Lazy<IFileLogger>(CreateDefaultFileWriter);

            if (!Directory.Exists(FileProvider.FolderPath))
            {
                Directory.CreateDirectory(FileProvider.FolderPath);
            }
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
            var fileName = FileProvider.TryResolveLastFile();

            if (fileName.IsEmpty()
                || new FileInfo(fileName).CreationTime.Date != Clock.Today)
            {
                fileName = FileProvider.ProduceNewFile();
            }

            return new FileLogger(fileName, append: true);
        }

        private PipeCollection<RollingFileContext> ProduceDefaultPipes()
        {
            Pipes = new PipeCollection<RollingFileContext>();

            this.UseDefaultWriterPipe()
                .UseDailyRollerPipe();

            return Pipes;
        }

        private RollingFileContext ProduceContext(LogMessage log)
        {
            return new RollingFileContext(this, log);
        }
    }
}