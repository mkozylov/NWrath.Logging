using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Text;

namespace NWrath.Logging.Test.ApiTests
{
    [TestFixture]
    public class LoggerJsonLoaderTests
    {
        [Test]
        public void LoggerJsonLoader_Load_WithComplexCtor()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var loggerTypeArg = typeof(EmptyLogger);
            var isEnabledArg = false;
            var json = GetBackgroundLoggerJsonWithComplexCtor(loggerTypeArg, isEnabledArg);
            var section = JObject.Parse(json);

            #endregion Arrange

            #region Act

            var logger = loader.Load(section) as BackgroundLogger;

            #endregion Act

            #region Assert

            Assert.IsNotNull(logger);
            Assert.AreEqual(isEnabledArg, logger.BaseLogger.IsEnabled);
            Assert.AreEqual(loggerTypeArg, logger.BaseLogger.GetType());

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithSimpleCtor()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var filePathArg = "path";
            var json = GetFileLoggerJsonWithSimpleCtor(filePathArg);
            var section = JObject.Parse(json);

            #endregion Arrange

            #region Act

            var logger = loader.Load(section) as FileLogger;

            #endregion Act

            #region Assert

            Assert.IsNotNull(logger);
            Assert.AreEqual(filePathArg, logger.FilePath);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithSimpleProperties()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var autoFlushProp1 = true;
            var autoFlushProp2 = false;
            var enabledProp1 = false;
            var enabledProp2 = true;
            var json1 = GetFileLoggerJsonWithSimpleProperties(autoFlushProp1, enabledProp1);
            var json2 = GetFileLoggerJsonWithSimpleProperties(autoFlushProp2, enabledProp2);
            var section1 = JObject.Parse(json1);
            var section2 = JObject.Parse(json2);

            #endregion Arrange

            #region Act

            var logger1 = loader.Load(section1) as FileLogger;
            var logger2 = loader.Load(section2) as FileLogger;

            #endregion Act

            #region Assert

            Assert.IsNotNull(logger1);
            Assert.AreEqual(autoFlushProp1, logger1.AutoFlush);
            Assert.AreEqual(enabledProp1, logger1.IsEnabled);
            Assert.IsNotNull(logger2);
            Assert.AreEqual(autoFlushProp2, logger2.AutoFlush);
            Assert.AreEqual(enabledProp2, logger2.IsEnabled);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithComplexProperties()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var serializerOutputTemplateProp = "{Message} {Level}";
            var json = GetFileLoggerJsonWithComplexProperties(serializerOutputTemplateProp);
            var section = JObject.Parse(json);
            var serializer = default(StringLogSerializer);

            #endregion Arrange

            #region Act

            var logger = (FileLogger)loader.Load(section);
            serializer = (StringLogSerializer)logger.Serializer;

            #endregion Act

            #region Assert

            Assert.AreNotEqual(StringLogSerializer.DefaultOutputTemplate, serializer.OutputTemplate);
            Assert.AreEqual(serializerOutputTemplateProp, serializer.OutputTemplate);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithoutCtor()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var json = GetEmptyLoggerJsonWithoutCtor();
            var section = JObject.Parse(json);

            #endregion Arrange

            #region Act

            var logger = loader.Load(section);

            #endregion Act

            #region Assert

            Assert.IsInstanceOf<EmptyLogger>(logger);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithCtorInjection()
        {
            #region Arrange

            var filePath = "path";

            var injectorMock = new Mock<IServiceProvider>();
            injectorMock.Setup(x => x.GetService(It.IsAny<Type>()))
                        .Returns<Type>(t => filePath);

            var loader = new LoggerJsonLoader { Injector = injectorMock.Object };
            var json = GetFileLoggerJsonWithCtorInjection();
            var section = JObject.Parse(json);

            #endregion Arrange

            #region Act

            var logger = loader.Load(section) as FileLogger;

            #endregion Act

            #region Assert

            Assert.IsNotNull(logger);
            Assert.AreEqual(filePath, logger.FilePath);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithPropertyInjection()
        {
            #region Arrange

            var filePath = "path";
            var serializer = new StringLogSerializer();

            var injectorMock = new Mock<IServiceProvider>();
            injectorMock.Setup(x => x.GetService(It.IsAny<Type>()))
                        .Returns<Type>(t => t == typeof(string) ? filePath : (object)serializer);

            var loader = new LoggerJsonLoader { Injector = injectorMock.Object };
            var json = GetFileLoggerJsonWithPropertyInjection();
            var section = JObject.Parse(json);

            #endregion Arrange

            #region Act

            var logger = loader.Load(section) as FileLogger;

            #endregion Act

            #region Assert

            Assert.IsNotNull(logger);
            Assert.AreEqual(filePath, logger.FilePath);
            Assert.AreSame(serializer, logger.Serializer);

            #endregion Assert
        }

        [Test]
        public void LoggerJsonLoader_Load_WithLevel()
        {
            #region Arrange

            var loader = new LoggerJsonLoader();
            var json1 = GetEmptyLoggerJsonWithLevel("Debug");
            var json2 = GetEmptyLoggerJsonWithLevel("Error");
            var section1 = JObject.Parse(json1);
            var section2 = JObject.Parse(json2);

            #endregion Arrange

            #region Act

            var logger1 = loader.Load(section1);
            var logger2 = loader.Load(section2);

            #endregion Act

            #region Assert

            Assert.IsInstanceOf<EmptyLogger>(logger1);
            Assert.IsInstanceOf<MinimumLogLevelVerifier>(logger1.RecordVerifier);
            Assert.AreEqual(LogLevel.Debug, logger1.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);
            Assert.IsInstanceOf<EmptyLogger>(logger2);
            Assert.IsInstanceOf<MinimumLogLevelVerifier>(logger2.RecordVerifier);
            Assert.AreEqual(LogLevel.Error, logger2.RecordVerifier.CastTo<MinimumLogLevelVerifier>().MinimumLevel);

            #endregion Assert
        }

        #region Internal

        private string GetEmptyLoggerJsonWithoutCtor()
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.EmptyLogger\": {")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetEmptyLoggerJsonWithLevel(string level)
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.EmptyLogger\": {")
                        .Append($"\"Level\": \"{level}\", ")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetFileLoggerJsonWithSimpleCtor(string filePath)
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.FileLogger\": {")
                        .Append("\"Level\": \"Debug\", ")
                        .Append("\"$ctor\": [")
                            .Append($"\"{filePath}\"")
                        .Append("]")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetBackgroundLoggerJsonWithComplexCtor(Type loggerType, bool isEnabled)
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.BackgroundLogger\": {")
                        .Append("\"$ctor\": [")
                            .Append($"{{ \"{loggerType.FullName}\": {{")
                              .Append($"\"IsEnabled\": {isEnabled.ToString().ToLower()}")
                            .Append("}}, ")
                            .Append("null, ")
                            .Append("null, ")
                            .Append("true, ")
                        .Append("]")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetFileLoggerJsonWithCtorInjection()
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.FileLogger\": {")
                        .Append("\"Level\": \"Debug\", ")
                        .Append("\"$ctor\": [")
                            .Append("{ \"$\": \"System.String\" }")
                        .Append("]")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetFileLoggerJsonWithPropertyInjection()
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.FileLogger\": {")
                        .Append("\"Serializer\": { \"$\": \"NWrath.Logging.StringLogSerializer\" }, ")
                        .Append("\"$ctor\": [")
                            .Append("\"path\"")
                        .Append("]")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetFileLoggerJsonWithSimpleProperties(bool autoFlush, bool isEnabled)
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.FileLogger\": {")
                        .Append($"\"AutoFlush\": {autoFlush.ToString().ToLower()}, ")
                        .Append($"\"IsEnabled\": {isEnabled.ToString().ToLower()}, ")
                        .Append("\"Level\": \"Debug\", ")
                        .Append("\"$ctor\": [")
                            .Append($"\"path\"")
                        .Append("]")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        private string GetFileLoggerJsonWithComplexProperties(string serializerOutputTemplate)
        {
            return new StringBuilder()
                .Append("{")
                    .Append("\"NWrath.Logging.FileLogger\": {")
                        .Append("\"$ctor\": [")
                            .Append($"\"path\"")
                        .Append("], ")
                        .Append("\"Serializer\": { \"NWrath.Logging.StringLogSerializer\": {")
                                            .Append($"\"OutputTemplate\": \"{serializerOutputTemplate}\"")
                                            .Append("}}")
                    .Append("}")
                .Append("}")
                .ToString();
        }

        #endregion Internal
    }
}