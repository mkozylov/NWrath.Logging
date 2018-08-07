using Newtonsoft.Json;
using NUnit.Framework;
using NWrath.Synergy.Common.Structs;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class JsonLogSerializerTests
    {
        [Test]
        public void JsonLogSerializer_Serialize()
        {
            var msg = new LogMessage
            {
                Message = "str",
                Exception = new NotImplementedException(),
                Extra = new StringSet { ["key"] = "value" }
            };
            var serializer = new JsonLogSerializer();

            var result = serializer.Serialize(msg);

            Assert.AreEqual(
                JsonConvert.SerializeObject(msg),
                result
                );
        }
    }
}