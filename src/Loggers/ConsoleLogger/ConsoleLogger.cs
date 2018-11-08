using System;

namespace NWrath.Logging
{
    public class ConsoleLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer
        {
            get => _serializer;

            set
            {
                _serializer = value ?? new ConsoleLogSerializer();

                if (typeof(IConsoleLogSerializer).IsAssignableFrom(_serializer.GetType()))
                {
                    _writerAction = m => _serializer.Serialize(m);
                }
                else
                {
                    _writerAction = m => Console.WriteLine(_serializer.Serialize(m));
                }
            }
        }

        private IStringLogSerializer _serializer;
        private Action<LogRecord> _writerAction;

        public ConsoleLogger()
        {
            Serializer = new ConsoleLogSerializer();
        }

        protected override void WriteRecord(LogRecord record)
        {
            _writerAction(record);
        }
    }
}