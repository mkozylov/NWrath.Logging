using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class FileLoggerTests
    {
        [Test]
        public void FileLogger_Log()
        {
            #region Arrange

            var msg = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var tempFile = Path.GetTempFileName();
            var serializer = new StringLogSerializer();
            var logger = new FileLogger(tempFile) { Serializer = serializer };
            var expected = serializer.Serialize(msg) + Environment.NewLine;
            var target = string.Empty;
            var error = default(Exception);

            #endregion Arrange

            #region Act

            try
            {
                using (logger)
                {
                    logger.Log(msg);
                }

                target = File.ReadAllText(tempFile);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                File.Exists(tempFile)
                    .If(true, x => File.Delete(tempFile));
            }

            #endregion Act

            #region Assert

            Assert.IsNull(error);
            Assert.AreEqual(expected, target);

            #endregion Assert
        }
    }
}