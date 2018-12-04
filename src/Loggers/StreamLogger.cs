using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Text;

namespace NWrath.Logging
{
    public class StreamLogger
         : LoggerBase
    {
        public Stream Writer { get => _writer; set { _writer = value ?? throw Errors.NULL_STREAM; } }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? new UTF8Encoding(false); } }

        private bool AutoFlush { get; set; } = false;

        private Stream _writer;
        private Encoding _encoding = new UTF8Encoding(false);
        private IStringLogSerializer _serializer = new StringLogSerializer();
        private bool _leaveOpen;

        public StreamLogger(
            Stream writer,
            bool autoFlush = false,
            bool leaveOpen = false
            )
        {
            Writer = writer;
            AutoFlush = autoFlush;
            _leaveOpen = leaveOpen;
        }

        ~StreamLogger()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (AutoFlush && Writer.CanWrite)
            {
                Writer.Flush();
            }

            if (!_leaveOpen)
            {
                Writer.Close();
                Writer.Dispose();
            }
        }

        protected override void WriteRecord(LogRecord record)
        {
            var msg = Serializer.Serialize(record) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            Writer.Write(data, 0, data.Length);

            if (AutoFlush)
            {
                Writer.Flush();
            }
        }
    }
}