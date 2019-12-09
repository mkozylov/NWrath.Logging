using NWrath.Synergy.Common.Structs;
using System.Text;

namespace NWrath.Logging
{
    public class RollingFileContext
    {
        public bool IsLoggerEnabled
        {
            get => _logger.IsEnabled; set => _logger.IsEnabled = value;
        }

        public IStringLogSerializer Serializer
        {
            get => _logger.Serializer; set => _logger.Serializer = value;
        }

        public Encoding Encoding
        {
            get => _logger.Encoding; set => _logger.Encoding = value;
        }

        public IRollingFileProvider FileProvider
        {
            get => _logger.FileProvider; set => _logger.FileProvider = value;
        }

        public FileContext LogFile { get; protected set; }

        public LogRecord[] Batch { get; set; }

        public Set Properties { get; protected set; }

        private RollingFileLogger _logger;

        public RollingFileContext(
            RollingFileLogger logger,
            FileLogger writer,
            LogRecord[] batch,
            Set properties = null
            )
        {
            _logger = logger;
            Batch = batch;
            Properties = properties ?? new Set();
            LogFile = new FileContext(writer);
        }

        public class FileContext
        {
            public long Size => _logger.FileSize;

            public string Path => _logger.FilePath;

            private FileLogger _logger;

            public FileContext(FileLogger logger)
            {
                _logger = logger;
            }

            public void Flush()
            {
                _logger.Flush();
            }

            public void Change(string filePath, bool append = true)
            {
                _logger.SetFile(filePath, append);
            }

            public void Write(LogRecord[] batch)
            {
                _logger.Log(batch);
            }
        }
    }
}