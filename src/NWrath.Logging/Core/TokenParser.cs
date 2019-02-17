using System.Collections.Generic;
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

        public Token[] Parse(string template)
        {
            var matches = _keyRegex.Matches(template);

            var temp = template;
            var tokens = new List<Token>(matches.Count);

            foreach (Match item in matches)
            {
                if (string.IsNullOrEmpty(temp))
                {
                    continue;
                }

                var leftStr = temp.Substring(0, temp.IndexOf(item.Value));

                temp = temp.Remove(0, leftStr.Length + item.Value.Length);

                if (!string.IsNullOrEmpty(leftStr))
                {
                    tokens.Add(new Token(leftStr));
                }

                tokens.Add(new Token(item.Groups[1].Value, item.Value));
            }

            if (!string.IsNullOrEmpty(temp))
            {
                tokens.Add(new Token(temp));
            }

            return tokens.ToArray();
        }
    }
}