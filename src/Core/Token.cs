using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public class Token
    {
        public string Value { get; set; }

        public string Key { get; set; }

        public bool IsString { get => Key == null; }

        public Token(string val, string key = null)
        {
            Key = key;

            Value = val;
        }
    }
}