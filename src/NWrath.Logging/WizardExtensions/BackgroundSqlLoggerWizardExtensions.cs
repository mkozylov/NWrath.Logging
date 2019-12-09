using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class BackgroundSqlLoggerWizardExtensions
    {
        #region BackgroundSql

        //1
        public static BackgroundLogger BackgroundSqlLogger(
            this LoggingWizardCharms charms,
            SqlLogSchema schema,
            ILogRecordVerifier recordVerifier
            )
        {
            var baseLogger = new SqlLogger(schema)
            {
                RecordVerifier = recordVerifier
            };

            return charms.BackgroundLogger(baseLogger);
        }

        //2
        public static BackgroundLogger BackgroundSqlLogger(
           this LoggingWizardCharms charms,
           string connectionString,
           ILogRecordVerifier recordVerifier
           )
        {
            return BackgroundSqlLogger(charms,
                new SqlLogSchema(connectionString),
                recordVerifier
                );
        }

        //3
        public static BackgroundLogger BackgroundSqlLogger(
            this LoggingWizardCharms charms,
            SqlLogSchema schema,
            LogLevel minLevel = LogLevel.Debug
            )
        {
            return BackgroundSqlLogger(
                charms,
                schema,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //4
        public static BackgroundLogger BackgroundSqlLogger(
           this LoggingWizardCharms charms,
           string connectionString,
           LogLevel minLevel = LogLevel.Debug
           )
        {
            return BackgroundSqlLogger(
                charms,
                connectionString,
                new MinimumLogLevelVerifier(minLevel)
                );
        }

        //5
        public static BackgroundLogger BackgroundSqlLogger(
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

            return BackgroundSqlLogger(
                charms,
                schema,
                recordVerifier
                );
        }

        //6
        public static BackgroundLogger BackgroundSqlLogger(
            this LoggingWizardCharms charms,
            LogLevel minLevel,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return BackgroundSqlLogger(
                charms,
                new MinimumLogLevelVerifier(minLevel),
                schemaApply
                );
        }

        //7
        public static BackgroundLogger BackgroundSqlLogger(
            this LoggingWizardCharms charms,
            Action<SqlLogSchemaConfig> schemaApply
            )
        {
            return BackgroundSqlLogger(
                charms,
                new MinimumLogLevelVerifier(LogLevel.Debug),
                schemaApply
                );
        }

        //8
        public static BackgroundLogger BackgroundSqlLogger(
           this LoggingWizardCharms charms,
           SqlLogSchema schema
           )
        {
            return BackgroundSqlLogger(
                charms,
                schema,
                new MinimumLogLevelVerifier(LogLevel.Debug)
                );
        }

        #endregion Db

    }
}
