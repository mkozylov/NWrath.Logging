using NWrath.Synergy.Common.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NWrath.Logging
{
    public class ConsoleLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer { get; set; }

        private Action<LogMessage> _writerAction;

        public ConsoleLogger(IStringLogSerializer serializer)
        {
            Serializer = serializer;

            _writerAction = m => Console.WriteLine(Serializer.Serialize(m));
        }

        public ConsoleLogger(IConsoleLogSerializer serializer)
        {
            Serializer = serializer;

            _writerAction = m => Serializer.Serialize(m);
        }

        public ConsoleLogger()
            : this(new ConsoleLogSerializer())
        {
        }

        protected override void WriteLog(LogMessage log)
        {
            _writerAction(log);
        }
    }
}