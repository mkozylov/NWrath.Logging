using System;

namespace NWrath.Logging
{
    public static class SqlConverter
    {
        public static string ToSqlString(DateTime val)
        {
            return "'" + val.ToIsoString('T') + "'";
        }

        public static string ToSqlString(DateTime? val)
        {
            return val == null ? "NULL" : ("'" + val.Value.ToIsoString('T') + "'");
        }

        public static string ToSqlString(bool val)
        {
            return Convert.ToInt32(val).ToString();
        }

        public static string ToSqlString(bool? val)
        {
            return val == null ? "NULL" : Convert.ToInt32(val.Value).ToString();
        }

        public static string ToSqlString(Enum val)
        {
            return val == null ? "NULL" : (Convert.ToInt32(val)).ToString();
        }

        public static string ToSqlString(string val)
        {
            return val == null ? "NULL" : ("'" + val.Replace("'", "''") + "'");
        }

        public static string ToSqlString(int val)
        {
            return val.ToString();
        }

        public static string ToSqlString(int? val)
        {
            return val == null ? "NULL" : val.Value.ToString();
        }

        public static string ToSqlString(long val)
        {
            return val.ToString();
        }

        public static string ToSqlString(long? val)
        {
            return val == null ? "NULL" : val.Value.ToString();
        }

        public static string ToSqlString(double val)
        {
            return val.ToString();
        }

        public static string ToSqlString(double? val)
        {
            return val == null ? "NULL" : val.Value.ToString();
        }

        public static string ToSqlString<TObj>(TObj val)
        {
            return val == null ? "NULL" : val.ToString();
        }
    }
}