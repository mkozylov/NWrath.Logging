using Newtonsoft.Json.Linq;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class LoadFromJsonLoggerWizardExtensions
    {
        #region LoadFromJson

        //1
        public static TLogger LoadFromJson<TLogger>(
           this LoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
            where TLogger : ILogger
        {
            return (TLogger)new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(filePath, sectionPath);
        }

        //2
        public static TLogger LoadFromJson<TLogger>(
           this LoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
             where TLogger : ILogger
        {
            return (TLogger)new LoggerJsonLoader
            {
                Injector = serviceProvider
            }.Load(loggingSection);
        }

        //3
        public static ILogger LoadFromJson(
           this LoggingWizardCharms charms,
           string filePath,
           string sectionPath,
           IServiceProvider serviceProvider = null
           )
        {
            return LoadFromJson<ILogger>(charms, filePath, sectionPath, serviceProvider);
        }

        //4
        public static ILogger LoadFromJson(
           this LoggingWizardCharms charms,
           JToken loggingSection,
           IServiceProvider serviceProvider = null
           )
        {
            return LoadFromJson<ILogger>(charms, loggingSection, serviceProvider);
        }

        #endregion LoadFromJson

    }
}
