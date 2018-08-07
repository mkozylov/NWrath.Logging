using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NWrath.Logging
{
    public class TokenParser
        : ITokenParser
    {
        public const string DefaultKeyPattern = @"{(\w+)}";

        public string KeyPattern
        {
            get => _keyPattern;
            set { _keyPattern = value; _keyRegex = new Regex(value, RegexOptions.Compiled | RegexOptions.IgnoreCase); }
        }

        private string _keyPattern;
        private Regex _keyRegex;

        public TokenParser()
        {
            KeyPattern = DefaultKeyPattern;
        }

        public virtual Token[] Parse(string template)
        {
            var matches = _keyRegex.Matches(template)
                                   .Cast<Match>();

            var temp = template;

            var tokens = matches.Aggregate(new List<Token>(), (list, item) =>
            {
                if (temp.NotEmpty())
                {
                    var leftStr = temp.Substring(0, temp.IndexOf(item.Value));

                    temp = temp.Remove(0, leftStr.Length + item.Value.Length);

                    if (leftStr.NotEmpty())
                    {
                        list.Add(new Token(leftStr));
                    }

                    list.Add(new Token(item.Groups[1].Value, item.Value));
                }

                return list;
            });

            if (temp.NotEmpty())
            {
                tokens.Add(new Token(temp));
            }

            return tokens.ToArray();
        }
    }
}