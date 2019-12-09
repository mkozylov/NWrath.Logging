using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class ConsoleLogSerializerTests
    {
        [Test]
        public void ConsoleLogSerializer_Serialize_OutputToStringAndConsole()
        {
            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var serializer = ConsoleLogSerializerBuilder.DefaultSerializer;
            var sw = new StringWriter();
            Console.SetOut(sw);

            #endregion Arrange

            #region Act

            var expected = serializer.Serialize(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(expected + sw.NewLine, sw.ToString());

            #endregion Assert
        }

        [Test]
        public void ConsoleLogSerializer_Serialize_CheckTokenColors()
        {
            if (!System.Diagnostics.Debugger.IsAttached) return;

            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };

            var serializerBuilder = new ConsoleLogSerializerBuilder();
            var consoleColors = new Dictionary<string, ConsoleColor>();

            foreach (var item in serializerBuilder.Formats.ToList())
            {
                serializerBuilder.Formats[item.Key] = m =>
                {
                    consoleColors[item.Key] = Console.ForegroundColor;

                    return item.Value(m);
                };
            }

            var serializer = serializerBuilder.BuildSerializer();
            var logger = new ConsoleLogger { Serializer = serializer };

            #endregion Arrange

            #region Act

            logger.Log(msg);
            logger.Dispose();
            #endregion Act

            #region Assert

            foreach (var item in consoleColors)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }

            Assert.AreEqual(
                serializerBuilder.Colors.Timestamp(msg),
                consoleColors[nameof(LogRecord.Timestamp)]
                );
            Assert.AreEqual(
               serializerBuilder.Colors.Message(msg),
               consoleColors[nameof(LogRecord.Message)]
               );
            Assert.AreEqual(
               serializerBuilder.Colors.Level(msg),
               consoleColors[nameof(LogRecord.Level)]
               );
            Assert.AreEqual(
               serializerBuilder.Colors.Exception(msg),
               consoleColors[nameof(LogRecord.Exception)]
               );

            #endregion Assert
        }
    }
}