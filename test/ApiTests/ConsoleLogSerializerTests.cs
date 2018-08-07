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

            var msg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var serializer = new ConsoleLogSerializer();
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
            #region Arrange

            var msg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var serializer = new ConsoleLogSerializer();
            var logger = new ConsoleLogger(serializer);
            var consoleColors = new Dictionary<string, ConsoleColor>();

            foreach (var item in serializer.Formats.ToList())
            {
                serializer.Formats[item.Key] = m =>
                {
                    consoleColors[item.Key] = Console.ForegroundColor;

                    return item.Value(m);
                };
            }

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(
                serializer.Colors.Timestamp(msg),
                consoleColors[nameof(LogMessage.Timestamp)]
                );
            Assert.AreEqual(
               serializer.Colors.Message(msg),
               consoleColors[nameof(LogMessage.Message)]
               );
            Assert.AreEqual(
               serializer.Colors.Level(msg),
               consoleColors[nameof(LogMessage.Level)]
               );
            Assert.AreEqual(
               serializer.Colors.Exception(msg),
               consoleColors[nameof(LogMessage.Exception)]
               );

            #endregion Assert
        }
    }
}