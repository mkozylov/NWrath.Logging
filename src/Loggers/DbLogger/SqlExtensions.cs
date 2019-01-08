using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class SqlExtensions
    {
        public static string ToSqlString(this DateTime val)
        {
            return "'" + val.ToIsoString('T') + "'";
        }

        public static string ToSqlString(this DateTime? val)
        {
            return val == null ? "NULL" : ("'" + val.Value.ToIsoString('T') + "'");
        }

        public static string ToSqlString(this bool val)
        {
            return Convert.ToInt32(val).ToString();
        }

        public static string ToSqlString(this bool? val)
        {
            return val == null ? "NULL" : Convert.ToInt32(val.Value).ToString();
        }

        public static string ToSqlString(this Enum val)
        {
            return val == null ? "NULL" : (Convert.ToInt32(val)).ToString();
        }

        public static string ToSqlString(this string val)
        {
            return val == null ? "NULL" : ("'" + val.ToString() + "'");
        }

        public static string ToSqlString(this int val)
        {
            return val.ToString();
        }

        public static string ToSqlString(this int? val)
        {
            return val == null ? "NULL" : val.Value.ToString();
        }

        public static string ToSqlString(this double val)
        {
            return val.ToString();
        }

        public static string ToSqlString(this double? val)
        {
            return val == null ? "NULL" : val.Value.ToString();
        }

        public static string ToSqlString<TObj>(this TObj val)
        {
            return val?.ToString() ?? "NULL";
        }
    }
}