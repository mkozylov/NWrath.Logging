using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Text;

namespace NWrath.Logging
{
    public class StreamLogger
         : LoggerBase, IDisposable
    {
        public Stream Writer { get => _writer; set { _writer = value ?? throw new ArgumentNullException(Errors.NULL_STREAM); } }

        public IStringLogSerializer Serializer { get => _serializer; set { _serializer = value ?? new StringLogSerializer(); } }

        public Encoding Encoding { get => _encoding; set { _encoding = value ?? Encoding.UTF8; } }

        private Stream _writer;
        private Encoding _encoding = Encoding.UTF8;
        private IStringLogSerializer _serializer = new StringLogSerializer();
        private bool _needFlush;
        private bool _leaveOpen;

        public StreamLogger(
            Stream writer,
            bool needFlush = true,
            bool leaveOpen = false
            )
        {
            Writer = writer;
            _needFlush = needFlush;
            _leaveOpen = leaveOpen;
        }

        ~StreamLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                Writer.Close();
                Writer.Dispose();
            }
        }

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log) + Environment.NewLine;

            var data = Encoding.GetBytes(msg);

            Writer.Write(data, 0, data.Length);

            if (_needFlush)
            {
                Writer.Flush();
            }
        }
    }
}