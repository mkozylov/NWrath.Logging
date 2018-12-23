﻿using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Text;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Common;
using NWrath.Synergy.Pipeline;
using System.Runtime.CompilerServices;

namespace NWrath.Logging
{
    public class RollingFileLogger
         : LoggerBase, IRollingFileLoggerInternal
    {
        public static IPipe<RollingFileContext> LogWriterPipe => new LambdaPipe<RollingFileContext>(
            (ctx, next) =>
            {
                next(ctx);

                if (ctx.IsLoggerEnabled)
                {
                    ctx.LogFile.Write(ctx.LogRecord);
                }
            }
        );

        IFileLogger IRollingFileLoggerInternal.Writer
        {
            get => _writer;
        }

        public PipeCollection<RollingFileContext> Pipes
        {
            get => _pipes;

            set { _pipes = value ?? new PipeCollection<RollingFileContext> { LogWriterPipe }; }
        }

        public IStringLogSerializer Serializer { get => _writer.Serializer; set { _writer.Serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _writer.Encoding; set { _writer.Encoding = value ?? new UTF8Encoding(false); } }

        public IRollingFileProvider FileProvider { get => _fileProvider; set { _fileProvider = value ?? throw Errors.NO_FILE_PROVIDER; } }

        private FileLogger _writer;
        private IRollingFileProvider _fileProvider;
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

            _writer = new FileLogger(string.Empty, FileMode.Append);
        }

        ~RollingFileLogger()
        {
            Dispose();
        }

        #endregion Ctor

        public override void Dispose()
        {
            _writer?.Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord record)
        {
            if (IsEnabled && VerifyRecord(record))
            {
                if (_writer.FilePath == string.Empty)
                {
                    SetDefaultWriter();
                }

                WriteRecord(record);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var ctx = ProduceContext(record);

            Pipes.Pipeline?.Perform(ctx);
        }

        private void SetDefaultWriter()
        {
            var fileName = FileProvider.TryResolveLastFile();

            if (fileName.IsEmpty()
                || new FileInfo(fileName).CreationTime.Date != Clock.Today)
            {
                fileName = FileProvider.ProduceNewFile();
            }

            _writer.SetFile(fileName, FileMode.Append);
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