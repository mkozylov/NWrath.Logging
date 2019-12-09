using Newtonsoft.Json.Linq;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundLoadFromJsonLoggerWizardExtensions
    {
        #region BackgroundLoadFromJson

        //1
        public static BackgroundLogger BackgroundLoadFromJson(
           this LoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
        {
            var baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(filePath, sectionPath);

            var bgLogger = baseLogger as BackgroundLogger;

            return bgLogger ?? charms.BackgroundLogger(baseLogger);
        }

        //2
        public static ILogger BackgroundLoadFromJson(
           this LoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
        {
            var baseLogger = new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(loggingSection);

            var bgLogger = baseLogger as BackgroundLogger;

            return bgLogger ?? charms.BackgroundLogger(baseLogger);
        }

        #endregion BackgroundLoadFromJson

    }
}
