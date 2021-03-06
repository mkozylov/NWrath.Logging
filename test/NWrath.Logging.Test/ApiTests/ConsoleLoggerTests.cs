﻿using NUnit.Framework;
using System;
using System.IO;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class ConsoleLoggerTest
    {
        [Test]
        public void ConsoleLogger_Log()
        {
            #region Arrange

            var serializer = ConsoleLogSerializerBuilder.DefaultSerializer;
            var logger = new ConsoleLogger { Serializer = serializer };
            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var sw = new StringWriter();
            var expected = serializer.Serialize(msg) + sw.NewLine;
            Console.SetOut(sw);

            #endregion Arrange

            #region Act

            logger.Log(msg);

            #endregion Act

            #region Assert

            Assert.AreEqual(
                    expected,
                    sw.ToString()
                    );

            #endregion Assert
        }
    }
}