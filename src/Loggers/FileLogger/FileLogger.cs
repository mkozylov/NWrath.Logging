using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Text;

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
                    SetWriter(_filePath, FileMode);
                }
                else
                {
                    Dispose();
                }
            }
        }

        public string FilePath { get => _filePath; set => SetWriter(value, FileMode); }

        public long FileSize
        {
            get => _writer.IsValueCreated
                    ? _writer.Value.Length
                    : new FileInfo(_filePath).If(x => x.Exists, t => t.Length, o => -1);
        }

        public FileMode FileMode { get; set; } = FileMode.Append;

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? Encoding.UTF8; } }

        private Lazy<FileStream> _writer;
        private bool _isEnabled = true;
        private string _filePath;
        private Encoding _encoding = Encoding.UTF8;
        private IStringLogSerializer _serializer = new StringLogSerializer();

        public FileLogger(
            string filePath
            )
        {
            _filePath = filePath;

            SetWriter(filePath, FileMode);
        }

        ~FileLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_writer?.IsValueCreated ?? false)
            {
                _writer.Value
                       .If(x => x.CanWrite, x => x.Flush());

                _writer.Value.Dispose();
            }
        }

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            _writer.Value.Write(data, 0, data.Length);

            _writer.Value.Flush();
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