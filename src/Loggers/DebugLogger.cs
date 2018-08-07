using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NWrath.Logging
{
    public class DebugLogger
         : LoggerBase
    {
        public IStringLogSerializer Serializer { get; set; }

        public DebugLogger(IStringLogSerializer serializer)
        {
            this.Serializer = serializer;

            System.Diagnostics.Debug.AutoFlush = true;
        }

        public DebugLogger()
            : this(new StringLogSerializer())
        {
        }

        protected override void WriteLog(LogMessage log)
        {
            var msg = Serializer.Serialize(log);

            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}