using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class LogTableColumnSchema
    {
        public virtual string Name { get; set; }

        public virtual string RawDbType { get; set; }

        public virtual string RawDefaultValue { get; set; }

        public virtual bool AllowNull { get; set; }

        public virtual bool IsKey { get; set; }

        public virtual bool IsInternal { get; set; }

        public virtual ILogSerializer Serializer { get; set; }

        public virtual string SerializerExpr
        {
            get { return _serializerExpr; }

            set
            {
                _serializerExpr = value;

                Serializer = (LambdaLogSerializer)_serializerExpr;
            }
        }

        private string _serializerExpr;

        public LogTableColumnSchema Clone()
        {
            return (LogTableColumnSchema)MemberwiseClone();
        }
    }
}