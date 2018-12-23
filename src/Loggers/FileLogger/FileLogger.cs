using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace NWrath.Logging
{
    public class FileLogger
         : LoggerBase, IFileLogger
    {
        public override bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                if (_isEnabled == value) return;

                _isEnabled = value;

                if (_isEnabled)
                {
                    SetFile(_filePath, FileMode.Append);
                }
                else
                {
                    Dispose();
                }
            }
        }

        public string FilePath { get => _filePath; }

        public long FileSize
        {
            get
            {
                if (_writer.IsValueCreated)
                {
                    return _writer.Value.Length;
                }

                var file = new FileInfo(_filePath);

                return file.Exists ? file.Length : -1;
            }
        }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? new UTF8Encoding(false); } }

        public bool AutoFlush { get; set; } = true;

        private Lazy<FileStream> _writer;
        private bool _isEnabled = true;
        private string _filePath;
        private Encoding _encoding = new UTF8Encoding(false);
        private IStringLogSerializer _serializer = new StringLogSerializer();

        public FileLogger(
            string filePath,
            FileMode fileMode = FileMode.Append
            )
        {
            _filePath = filePath;

            SetFile(filePath, fileMode);
        }

        ~FileLogger()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (_writer?.IsValueCreated ?? false)
            {
                if (_writer.Value.CanWrite)
                {
                    _writer.Value.Flush();
                }

                _writer.Value.Dispose();
            }
        }

        public void Flush()
        {
            if (_writer.IsValueCreated && _writer.Value.CanWrite)
            {
                _writer.Value.Flush();
            }
        }

        public void SetFile(string fileName, FileMode fileMode = FileMode.Append)
        {
            _filePath = fileName;

            Dispose();

            _writer = new Lazy<FileStream>(() =>
            {
                return new FileStream(
                    FilePath,
                    fileMode,
                    FileAccess.Write,
                    FileShare.Read
                    );
            });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(byte[] data)
        {
            if (!IsEnabled || data.Length == 0 || !_writer.Value.CanWrite)
            {
                return;
            }

            _writer.Value.Write(data, 0, data.Length);

            if (AutoFlush)
            {
                _writer.Value.Flush();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0 || !_writer.Value.CanWrite)
            {
                return;
            }

            var sb = new StringBuilder();

            foreach (var record in batch)
            {
                if (VerifyRecord(record))
                {
                    sb.AppendLine(Serializer.Serialize(record));
                }
            }

            var data = _encoding.GetBytes(sb.ToString());

            _writer.Value.Write(data, 0, data.Length);

            if (AutoFlush)
            {
                _writer.Value.Flush();
            }
        }

        public override void Log(LogRecord record)
        {
            if (IsEnabled && _writer.Value.CanWrite && VerifyRecord(record))
            {
                WriteRecord(record);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var msg = Serializer.Serialize(record) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            _writer.Value.Write(data, 0, data.Length);

            if (AutoFlush)
            {
                _writer.Value.Flush();
            }
        }
    }
}