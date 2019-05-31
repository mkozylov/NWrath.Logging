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
            if (!System.Diagnostics.Debugger.IsAttached) return;

            #region Arrange

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var serializer = new ConsoleLogSerializer();
            var logger = new ConsoleLogger { Serializer = serializer };
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
            logger.Dispose();
            #endregion Act

            #region Assert

            foreach (var item in consoleColors)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }

            Assert.AreEqual(
                serializer.Colors.Timestamp(msg),
                consoleColors[nameof(LogRecord.Timestamp)]
                );
            Assert.AreEqual(
               serializer.Colors.Message(msg),
               consoleColors[nameof(LogRecord.Message)]
               );
            Assert.AreEqual(
               serializer.Colors.Level(msg),
               consoleColors[nameof(LogRecord.Level)]
               );
            Assert.AreEqual(
               serializer.Colors.Exception(msg),
               consoleColors[nameof(LogRecord.Exception)]
               );

            #endregion Assert
        }
    }
}