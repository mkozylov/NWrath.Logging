using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

                if (_fileInfo != null)
                {
                    _fileInfo.Refresh();

                    return _fileInfo.Exists ? _fileInfo.Length : -1;
                }

                return -1;
            }
        }

        public IStringLogSerializer Serializer
        {
            get => _serializer;
            set => _serializer = value ?? StringLogSerializerBuilder.DefaultSerializer;
        }

        public Encoding Encoding
        {
            get => _encoding;
            set => _encoding = value ?? new UTF8Encoding(false);
        }

        public bool AutoFlush { get; set; }

        private Lazy<FileStream> _writer;
        private bool _isEnabled = true;
        private Encoding _encoding = new UTF8Encoding(false);
        private IStringLogSerializer _serializer = StringLogSerializerBuilder.DefaultSerializer;
        private FileInfo _fileInfo;
        private FileSystemWatcher _fileDeleteWatcher;

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

            _fileDeleteWatcher?.Dispose();
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
                var fs = new FileStream(
                            FilePath,
                            append ? FileMode.Append : FileMode.Create,
                            FileAccess.Write,
                            FileShare.ReadWrite | FileShare.Delete
                            );

                _fileInfo = new FileInfo(fs.Name);

                if (_fileDeleteWatcher != null)
                {
                    _fileDeleteWatcher.Dispose();
                }

                _fileDeleteWatcher = new FileSystemWatcher(_fileInfo.DirectoryName);
                _fileDeleteWatcher.NotifyFilter = NotifyFilters.FileName;
                _fileDeleteWatcher.Filter = _fileInfo.Name;
                _fileDeleteWatcher.Deleted +=(s, e) =>
                {
                    if (IsEnabled && _writer.IsValueCreated && _writer.Value.CanWrite)
                    {
                        SetFile(fs.Name, true);
                    }
                };

                _fileDeleteWatcher.EnableRaisingEvents = true;

                return fs;
            });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(byte[] data)
        {
            if (!IsEnabled || data.Length == 0)
            {
                return;
            }

            WriteBytes(data);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord[] batch)
        {
            if (!IsEnabled || batch.Length == 0)
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

            WriteBytes(data);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Log(LogRecord record)
        {
            if (IsEnabled
                && RecordVerifier.Verify(record)
                )
            {
                WriteRecord(record);
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var msg = Serializer.Serialize(record) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            WriteBytes(data);
        }

        private void WriteBytes(byte[] data)
        {
            if (!_writer.Value.CanWrite)
            {
                throw new ObjectDisposedException("writer");
            }

            _writer.Value.Write(data, 0, data.Length);
          
            if (AutoFlush)
            {
                _writer.Value.Flush();
            }
        }
    }
}