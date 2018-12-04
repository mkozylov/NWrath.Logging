using NUnit.Framework;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LambdaLogSerializerTests
    {
        [Test]
        public void LambdaLogSerializer_Serialize()
        {
            var msg = new LogRecord { Message = "str", Exception = new NotImplementedException() };
            var serializer = new LambdaLogSerializer(m => m.Message);

            var result = serializer.Serialize(msg);

            Assert.AreEqual($"{msg.Message}", result);
        }
    }
}