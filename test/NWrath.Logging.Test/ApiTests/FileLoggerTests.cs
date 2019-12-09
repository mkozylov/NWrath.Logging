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

            var msg = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var tempFile = Path.GetTempFileName();
            var serializer = StringLogSerializerBuilder.DefaultSerializer;
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

        [Test]
        public void FileLogger_LogWithAutoFlush()
        {
            #region Arrange

            var msg1 = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var msg2 = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str2",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var tempFile = Path.GetTempFileName();
            var serializer = StringLogSerializerBuilder.DefaultSerializer;
            var logger = new FileLogger(tempFile) { AutoFlush = true, Serializer = serializer };
            var expected1 = serializer.Serialize(msg1) + Environment.NewLine;
            var expected2 = serializer.Serialize(msg2) + Environment.NewLine;
            var target1 = string.Empty;
            var target2 = string.Empty;
            var error = default(Exception);

            #endregion Arrange

            #region Act

            try
            {
                logger.Log(msg1);
                target1 = NonBlockRead(tempFile);
                logger.IsEnabled = false;
                File.Delete(tempFile);

                logger.IsEnabled = true;
                logger.Log(msg2);
                target2 = NonBlockRead(tempFile);
                logger.IsEnabled = false;
                File.Delete(tempFile);

                logger.Dispose();
            }
            catch (Exception ex)
            {
                error = ex;

                File.Exists(tempFile)
                  .If(true, x => File.Delete(tempFile));
            }

            #endregion Act

            #region Assert

            Assert.IsNull(error);
            Assert.AreEqual(expected1, target1);
            Assert.AreEqual(expected2, target2);
            Assert.AreEqual(false, File.Exists(tempFile));

            #endregion Assert
        }

        [Test]
        public void FileLogger_LogWithoutAutoFlush()
        {
            #region Arrange

            var msg1 = new LogRecord
            {
                Timestamp = DateTime.Now,
                Message = "str",
                Level = LogLevel.Error,
                Exception = new Exception("Ex")
            };
            var tempFile = Path.GetTempFileName();
            var serializer = StringLogSerializerBuilder.DefaultSerializer;
            var logger = new FileLogger(tempFile) { AutoFlush = false, Serializer = serializer };
            var expected = serializer.Serialize(msg1) + Environment.NewLine;
            var targetBeforeFlush = string.Empty;
            var targetAfterFlush = string.Empty;
            var error = default(Exception);

            #endregion Arrange

            #region Act

            try
            {
                logger.Log(msg1);
                targetBeforeFlush = NonBlockRead(tempFile);
                logger.Flush();

                targetAfterFlush = NonBlockRead(tempFile);

                //throw file locked was here
                File.ReadAllText(tempFile);
            }
            catch (Exception ex)
            {
                logger.Dispose();
                error = ex;
            }

            #endregion Act

            #region Assert

            Assert.IsNotNull(error);
            Assert.IsTrue(error.Message.StartsWith("Процесс не может получить доступ к файлу"));
            Assert.AreEqual(string.Empty, targetBeforeFlush);
            Assert.AreEqual(expected, targetAfterFlush);
            Assert.AreEqual(true, File.Exists(tempFile));

            File.Exists(tempFile)
                 .If(true, x => File.Delete(tempFile));

            #endregion Assert
        }

        #region Internal

        private static string NonBlockRead(string tempFile)
        {
            using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        } 

        #endregion
    }
}