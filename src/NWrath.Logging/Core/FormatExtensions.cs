using System;

namespace NWrath.Logging
{
    public static class FormatExtensions
    {
        public static string ToIsoString(this DateTime dateTime, char separator = ' ')
        {
            var chars = new char[23];

            Write4Chars(chars, 0, dateTime.Year);
            chars[4] = '-';
            Write2Chars(chars, 5, dateTime.Month);
            chars[7] = '-';
            Write2Chars(chars, 8, dateTime.Day);
            chars[10] = separator;
            Write2Chars(chars, 11, dateTime.Hour);
            chars[13] = ':';
            Write2Chars(chars, 14, dateTime.Minute);
            chars[16] = ':';
            Write2Chars(chars, 17, dateTime.Second);
            chars[19] = '.';
            Write2Chars(chars, 20, dateTime.Millisecond / 10);
            chars[22] = Digit(dateTime.Millisecond % 10);

            return new string(chars);
        }

        private static void Write4Chars(char[] chars, int offset, int value)
        {
            chars[offset] = Digit(value / 1000);
            chars[offset + 1] = Digit(value / 100 % 10);
            chars[offset + 2] = Digit(value / 10 % 10);
            chars[offset + 3] = Digit(value % 10);
        }

        private static void Write2Chars(char[] chars, int offset, int value)
        {
            chars[offset] = Digit(value / 10);
            chars[offset + 1] = Digit(value % 10);
        }

        private static char Digit(int value)
        {
            return (char)(value + '0');
        }
    }
}