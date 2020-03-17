using FastExpressionCompiler;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NWrath.Logging
{
    public class SqlLogSerializer<TType>
        : ISqlStringLogSerializer<TType>
    {
        public static Func<TType, string> SqlStringConverter { get; set; }

        private Func<LogRecord, string> _serializeFunc;

        static SqlLogSerializer()
        {
            SqlStringConverter = typeof(SqlTypeConverter).GetMethod(
                                   nameof(SqlTypeConverter.ToSqlString),
                                   new[] { typeof(TType) }
                                   )
                               .CreateDelegate(Expression.GetFuncType(typeof(TType), typeof(string)))
                               .CastTo<Func<TType, string>>();
        }

        public SqlLogSerializer(Func<LogRecord, TType> extractValueFunc)
        {
            _serializeFunc = r => SqlStringConverter(extractValueFunc(r));
        }

        public string Serialize(LogRecord record)
        {
            var result = _serializeFunc(record);

            return result;
        }
    }
}