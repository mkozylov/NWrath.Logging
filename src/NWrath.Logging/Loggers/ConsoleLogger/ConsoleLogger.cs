﻿using System;

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
                _serializer = value ?? ConsoleLogSerializerBuilder.DefaultSerializer;

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
            Serializer = ConsoleLogSerializerBuilder.DefaultSerializer;
        }

        protected override void WriteRecord(LogRecord record)
        {
            _writerAction(record);
        }
    }
}