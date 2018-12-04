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
            var serializer = new StringLogSerializer { OutputTemplate = "{Message}{Unknown}{Level}" };
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
            var serializer = new StringLogSerializer { OutputTemplate = StringLogSerializer.DefaultOutputTemplate };
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
            var serializer = new StringLogSerializer { OutputTemplate = "{Num})\t{Message}\t{Level}" };
            serializer.Formats["Num"] = m => "1";
            serializer.Formats.Message = m => $"Message: {m.Message}";
            serializer.Formats.Level = m => ((int)m.Level).ToString();
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
            var serializer = new StringLogSerializer();
            var msg = new LogRecord
            {
                Message = "str",
                Level = LogLevel.Debug
            };

            serializer.OutputTemplate = StringLogSerializer.DefaultOutputTemplate;
            var result1 = serializer.Serialize(msg);

            serializer.OutputTemplate = "{Message}";
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

        [Test]
        public void StringLogSerializer_FormatsDemandChanges()
        {
            var serializer = new StringLogSerializer();
            var msg = new LogRecord
            {
                Timestamp = DateTime.Today,
                Message = "str",
                Level = LogLevel.Debug,
                Exception = new NotImplementedException()
            };

            serializer.Formats.Timestamp = m => nameof(RecordFormatStore.Timestamp);
            Assert.AreEqual(
                $"{nameof(RecordFormatStore.Timestamp)} [{msg.Level}] {msg.Message}{(msg.Exception != null ? (Environment.NewLine + msg.Exception) : "")}",
                serializer.Serialize(msg)
                );

            serializer.Formats.Level = m => nameof(RecordFormatStore.Level);
            Assert.AreEqual(
               $"{nameof(RecordFormatStore.Timestamp)} [{nameof(RecordFormatStore.Level)}] {msg.Message}{(msg.Exception != null ? (Environment.NewLine + msg.Exception) : "")}",
               serializer.Serialize(msg)
               );

            serializer.Formats.Message = m => nameof(RecordFormatStore.Message);
            Assert.AreEqual(
                $"{nameof(RecordFormatStore.Timestamp)} [{nameof(RecordFormatStore.Level)}] {nameof(RecordFormatStore.Message)}{(msg.Exception != null ? (Environment.NewLine + msg.Exception) : "")}",
                serializer.Serialize(msg)
                );

            serializer.Formats.Exception = m => nameof(RecordFormatStore.Exception);
            Assert.AreEqual(
               $"{nameof(RecordFormatStore.Timestamp)} [{nameof(RecordFormatStore.Level)}] {nameof(RecordFormatStore.Message)}{Environment.NewLine}{nameof(RecordFormatStore.Exception)}",
               serializer.Serialize(msg)
               );

            serializer.Formats = new RecordFormatStore();
            Assert.AreEqual(
                SerializeToDefaultOutputTemplate(msg),
                serializer.Serialize(msg)
                );
        }

        #region Internal

        private string SerializeToDefaultOutputTemplate(LogRecord msg)
        {
            return $"{msg.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.ff")} [{msg.Level}] {msg.Message}{(msg.Exception != null ? (Environment.NewLine + msg.Exception) : "")}";
        }

        #endregion Internal
    }
}