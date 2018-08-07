using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections;
using NWrath.Synergy.Common.Extensions;
using NWrath.Synergy.Common.Extensions.Collections;

namespace NWrath.Logging
{
    public class TokenFormatStore
        : Dictionary<string, Func<LogMessage, string>>, ITokenFormatStore
    {
        public virtual Func<LogMessage, string> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public virtual Func<LogMessage, string> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public virtual Func<LogMessage, string> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public virtual Func<LogMessage, string> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public virtual Func<LogMessage, string> Extra
        {
            get => this[nameof(Extra)];
            set => this[nameof(Extra)] = value;
        }

        public event EventHandler Updated;

        public TokenFormatStore()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            InitFormats();
        }

        ~TokenFormatStore()
        {
            Updated?.GetInvocationList()
                   ?.Each(x => Updated -= (EventHandler)x);
        }

        public new virtual ITokenFormatStore Add(string key, Func<LogMessage, string> val)
        {
            base.Add(key, val);

            OnUpdated();

            return this;
        }

        public virtual Func<LogMessage, string> this[string key, Func<LogMessage, string> defaultFactory = null]
        {
            get => (ContainsKey(key) ? base[key] : (defaultFactory));
            set { base[key] = value; OnUpdated(); }
        }

        public new virtual Func<LogMessage, string> this[string key]
        {
            get => (ContainsKey(key) ? base[key] : (null));
            set { base[key] = value; OnUpdated(); }
        }

        public virtual string this[string key, LogMessage log]
        {
            get => (ContainsKey(key) ? base[key] : (m => key))(log);
        }

        protected virtual void InitFormats()
        {
            base[nameof(Timestamp)] = DefaultTimestampFormat;
            base[nameof(Message)] = DefaultMessageFormat;
            base[nameof(Level)] = DefaultLevelTypeFormat;
            base[nameof(Exception)] = DefaultExceptionFormat;
            base[nameof(Extra)] = DefaultExtraFormat;
            base["NewLine"] = m => Environment.NewLine;
            base["ExNewLine"] = (m => m.Exception == null ? "" : Environment.NewLine);
        }

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual string DefaultTimestampFormat(LogMessage log)
        {
            return log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }

        protected virtual string DefaultMessageFormat(LogMessage log)
        {
            return log.Message;
        }

        protected virtual string DefaultLevelTypeFormat(LogMessage log)
        {
            return log.Level.ToString();
        }

        protected virtual string DefaultExceptionFormat(LogMessage log)
        {
            return log.Exception?.ToString() ?? "";
        }

        protected virtual string DefaultExtraFormat(LogMessage log)
        {
            return log.Extra.Count == 0
                ? string.Empty
                : log.Extra.AsJson();
        }
    }
}