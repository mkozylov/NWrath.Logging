using NUnit.Framework;
using System.Linq;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class TokenParserTests
    {
        [Test]
        public void TokenParser_ParseTokensWithText()
        {
            GeneralAssert(
                new TokenParser().Parse("{token1}text1{token2}text2"),
                new[] { "token1", "token2" },
                new[] { "text1", "text2" }
                );
        }

        [Test]
        public void TokenParser_ParseTokensWithKeyPattern()
        {
            GeneralAssert(
                 new TokenParser { KeyPattern = @"<=(\w+)=>" }.Parse("<=token1=>{token2}<=token3=>text1<=token4=>"),
                 new[] { "token1", "token3", "token4" },
                 new[] { "{token2}", "text1" }
                 );
        }

        [Test]
        public void TokenParser_ParseTokensWithKeyEscape()
        {
            GeneralAssert(
                 new TokenParser().Parse("{token1}text1{{token2}}"),
                 new[] { "token1", "token2" },
                 new[] { "text1{", "}" }
                 );
        }

        [Test]
        public void TokenParser_ParseTokensWithKeySpace()
        {
            GeneralAssert(
                new TokenParser().Parse("{token1 } text1 {token2}text2"),
                new[] { "token2" },
                new[] { "{token1 } text1 ", "text2" }
                );
        }

        private void GeneralAssert(Token[] tokens, string[] tokenKeys, string[] textStrs)
        {
            Assert.AreEqual(tokenKeys.Length + textStrs.Length, tokens.Length);
            Assert.AreEqual(tokenKeys.Length, tokens.Count(x => !x.IsLiteral));
            Assert.AreEqual(textStrs.Length, tokens.Count(x => x.IsLiteral));
            Assert.That(tokens.Where(x => !x.IsLiteral)
                              .All(x => tokenKeys.Contains(x.Value)));
            Assert.That(tokens.Where(x => x.IsLiteral)
                              .All(x => textStrs.Contains(x.Value)));
        }
    }
}