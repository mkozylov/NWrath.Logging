using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;
using NWrath.Synergy.Formatting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NWrath.Logging
{
    public class RecordFormatStore
        : StringFormatStore<LogRecord>
    {
        public Func<LogRecord, string> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public Func<LogRecord, string> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public Func<LogRecord, string> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public Func<LogRecord, string> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public Func<LogRecord, string> Extra
        {
            get => this[nameof(Extra)];
            set => this[nameof(Extra)] = value;
        }

        public RecordFormatStore()
            : base()
        {
            InitFormats();
        }

        #region Internal

        private void InitFormats()
        {
            this[nameof(Timestamp)] = DefaultTimestampFormat;
            this[nameof(Message)] = DefaultMessageFormat;
            this[nameof(Level)] = DefaultLevelTypeFormat;
            this[nameof(Exception)] = DefaultExceptionFormat;
            this[nameof(Extra)] = DefaultExtraFormat;
            this["exnl"] = (m => m.Exception == null ? "" : Environment.NewLine);
        }

        private string DefaultTimestampFormat(LogRecord record)
        {
            return record.Timestamp.ToIsoString();
        }

        private string DefaultMessageFormat(LogRecord record)
        {
            return record.Message;
        }

        private string DefaultLevelTypeFormat(LogRecord record)
        {
            return record.Level.ToString();
        }

        private string DefaultExceptionFormat(LogRecord record)
        {
            return record.Exception?.ToString() ?? "";
        }

        private string DefaultExtraFormat(LogRecord record)
        {
            return record.Extra.Count == 0
                ? string.Empty
                : record.Extra.AsJson();
        } 

        #endregion
    }
}