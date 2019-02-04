using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace NWrath.Logging
{
    public class FileLogger
         : LoggerBase
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
                    SetFile(FilePath, true);
                }
                else
                {
                    Dispose();
                }
            }
        }

        public string FilePath { get; protected set; }

        public long FileSize
        {
            get
            {
                if (_writer.IsValueCreated)
                {
                    return _writer.Value.Length;
                }

                var file = new FileInfo(FilePath);

                return file.Exists ? file.Length : -1;
            }
        }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? new UTF8Encoding(false); } }

        public bool AutoFlush { get => _autoFlush; set { _autoFlush = value; _writeBytesAction = value ? AllWriteBytes : (Action<byte[]>)WriteBytes; } }

        private Lazy<FileStream> _writer;
        private bool _isEnabled = true;
        private Encoding _encoding = new UTF8Encoding(false);
        private IStringLogSerializer _serializer = new StringLogSerializer();
        private bool _autoFlush;
        private Action<byte[]> _writeBytesAction;

        public FileLogger(
            string filePath,
            bool append = true
            )
        {
            FilePath = filePath;
            AutoFlush = true;

            SetFile(filePath, append);
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

        public void SetFile(string fileName, bool append)
        {
            FilePath = fileName;

            Dispose();

            _writer = new Lazy<FileStream>(() =>
            {
                return new FileStream(
                    FilePath,
                    append ? FileMode.Append : FileMode.Create,
                    FileAccess.Write,
                    FileShare.ReadWrite
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

            _writeBytesAction(data);
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
                if (RecordVerifier.Verify(record))
                {
                    sb.AppendLine(Serializer.Serialize(record));
                }
            }

            var data = _encoding.GetBytes(sb.ToString());

            _writeBytesAction(data);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord record)
        {
            if (IsEnabled && _writer.Value.CanWrite && RecordVerifier.Verify(record))
            {
                WriteRecord(record);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var msg = Serializer.Serialize(record) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            _writeBytesAction(data);
        }

        private void WriteBytes(byte[] data)
        {
            _writer.Value.Write(data, 0, data.Length);
        }

        private void AllWriteBytes(byte[] data)
        {
            _writer.Value.Write(data, 0, data.Length);

            _writer.Value.Flush();

            SetFile(FilePath, true);
        }
    }
}