using NUnit.Framework;
using System;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class StringLogSerializerTests
    {
        [Test]
        public void StringLogSerializer_OutputTemplateWithUnknownToken()
        {
            var serializer = new StringLogSerializerBuilder { OutputTemplate = "{Message}{Unknown}{Level}" }.BuildSerializer();
            var msg = new LogRecord
            {
                Message = "str",
                Level = LogLevel.Debug
            };

            var result = serializer.Serialize(msg);

            Assert.AreEqual(
                $"{msg.Message}{{Unknown}}{msg.Level}",
                result
                );
        }

        [Test]
        public void StringLogSerializer_DefaultOutputTemplate()
        {
            var serializer = new StringLogSerializerBuilder { OutputTemplate = StringLogSerializerBuilder.DefaultOutputTemplate }.BuildSerializer();
            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Debug,
                Exception = new Exception("err")
            };

            var result = serializer.Serialize(msg);

            Assert.AreEqual(
                SerializeToDefaultOutputTemplate(msg),
                result
                );
        }

        [Test]
        public void StringLogSerializer_CustomOutputTemplate()
        {
            var serializerBuilder = new StringLogSerializerBuilder { OutputTemplate = "{Num})\t{Message}\t{Level}" };
            serializerBuilder.Formats["Num"] = m => "1";
            serializerBuilder.Formats.Message = m => $"Message: {m.Message}";
            serializerBuilder.Formats.Level = m => ((int)m.Level).ToString();

            var serializer = serializerBuilder.BuildSerializer();
            var msg = new LogRecord
            {
                Message = "str",
                Level = LogLevel.Debug
            };

            var result = serializer.Serialize(msg);

            Assert.AreEqual(
                $"1)\tMessage: {msg.Message}\t{(int)msg.Level}",
                result
                );
        }

        [Test]
        public void StringLogSerializer_OutputTemplateDemandChanges()
        {
            var serializerBuilder = new StringLogSerializerBuilder();

            var msg = new LogRecord
            {
                Message = "str",
                Level = LogLevel.Debug
            };

            serializerBuilder.OutputTemplate = StringLogSerializerBuilder.DefaultOutputTemplate;
            var serializer = serializerBuilder.BuildSerializer();
            var result1 = serializer.Serialize(msg);

            serializerBuilder.OutputTemplate = "{Message}";
            serializer = serializerBuilder.BuildSerializer();
            var result2 = serializer.Serialize(msg);

            Assert.AreEqual(
                SerializeToDefaultOutputTemplate(msg),
                result1
                );
            Assert.AreEqual(
                $"{msg.Message}",
                result2
                );
        }

        #region Internal

        private string SerializeToDefaultOutputTemplate(LogRecord msg)
        {
            return $"{msg.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{msg.Level}] {msg.Message}{(msg.Exception != null ? (Environment.NewLine + msg.Exception) : "")}";
        }

        #endregion Internal
    }
}