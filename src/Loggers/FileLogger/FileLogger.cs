using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging
{
    public class FileLogger
         : LoggerBase, IDisposable, IFileLogger
    {
        public override bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                _isEnabled = value;

                if (_isEnabled)
                {
                    SetWriter(_filePath, FileMode.Append);
                }
                else
                {
                    Dispose();
                }
            }
        }

        public string FilePath { get => _filePath; set => SetWriter(value, FileMode.Append); }

        public long FileSize
        {
            get => _writer.IsValueCreated
                    ? _writer.Value.Length
                    : new FileInfo(_filePath).If(x => x.Exists, t => t.Length, o => -1);
        }

        public IStringLogSerializer Serializer { get; set; }

        public Encoding Encoding { get; set; }

        private Lazy<FileStream> _writer;
        private bool _isEnabled = true;
        private string _filePath;

        public FileLogger(
            string filePath,
            IStringLogSerializer serializer,
            Encoding encoding,
            bool append = false
            )
        {
            _filePath = filePath;
            Serializer = serializer ?? new StringLogSerializer();
            Encoding = encoding ?? Encoding.UTF8;

            SetWriter(filePath, append ? FileMode.Append : FileMode.OpenOrCreate);
        }

        public FileLogger(string filePath, IStringLogSerializer serializer, bool append = false)
           : this(filePath, serializer, Encoding.UTF8, append)
        {
        }

        public FileLogger(string filePath, Encoding encoding, bool append = false)
            : this(filePath, new StringLogSerializer(), encoding, append)
        {
        }

        public FileLogger(string filePath, bool append = false)
            : this(filePath, new StringLogSerializer(), Encoding.UTF8, append)
        {
        }

        ~FileLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_writer?.IsValueCreated ?? false)
            {
                _writer.Value.Dispose();
            }

            _writer = null;
        }

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            _writer?.Value?.Write(data, 0, data.Length);

            _writer?.Value?.Flush();
        }

        private void SetWriter(string fileName, FileMode mode)
        {
            Dispose();

            _filePath = fileName;

            _writer = new Lazy<FileStream>(() =>
            {
                return new FileStream(
                    fileName,
                    mode,
                    FileAccess.Write,
                    FileShare.ReadWrite | FileShare.Delete,
                    bufferSize: 4 * 1024,
                    useAsync: false
                    );
            });
        }
    }
}