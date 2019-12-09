using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class SqlLoggerWizardExtensions
    {
        #region Sql

        //1
        public static SqlLogger SqlLogger(
            this LoggingWizardCharms charms,
            SqlLogSchema schema,
            ILogRecordVerifier recordVerifier
            )
        {
            return new SqlLogger(schema)
            {
                RecordVerifier = recordVerifier
            };
        }

        //2
        public static SqlLogger SqlLogger(
           this LoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier
           )
        {
            return SqlLogger(charms,
                new SqlLogSchema(connectionString),
                recordVerifier
                );
        }

        //3
        public static SqlLogger SqlLogger(
            this LoggingWizardCharms charms,
            SqlLogSchema schema,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            return SqlLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static SqlLogger SqlLogger(
           this LoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return SqlLogger(
                charms, 
                connectionString, 
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //5
        public static SqlLogger SqlLogger(
            this LoggingWizardCharms charms,
            ILogRecordVerifier recordVerifier,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            var args = new SqlLogSchemaConfig();

            schemaApply(args);

            var schema = new SqlLogSchema(
                args.ConnectionString, 
                args.TableName, 
                args.InitScript, 
                args.Columns
                );

            return SqlLogger(
                charms,
                schema,
                recordVerifier
                );
        }

        //6
        public static SqlLogger SqlLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return SqlLogger(
                charms, 
                new MinimumLogLevelVerifier(minLevel), 
                schemaApply
                );
        }

        //7
        public static SqlLogger SqlLogger(
            this LoggingWizardCharms charms,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return SqlLogger(
                charms, 
                new MinimumLogLevelVerifier(LogLevel.Debug), 
                schemaApply
                );
        }

        //8
        public static SqlLogger SqlLogger(
           this LoggingWizardCharms charms,
           SqlLogSchema schema
           )
        {
            return SqlLogger(
                charms, 
                schema, 
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Sql

    }
}
