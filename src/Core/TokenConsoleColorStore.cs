using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections;
using NWrath.Synergy.Common.Extensions;

namespace NWrath.Logging
{
    public class TokenConsoleColorStore
        : Dictionary<string, Func<LogMessage, ConsoleColor>>, ITokenConsoleColorStore
    {
        public virtual Func<LogMessage, ConsoleColor> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public virtual Func<LogMessage, ConsoleColor> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public virtual Func<LogMessage, ConsoleColor> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public virtual Func<LogMessage, ConsoleColor> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public virtual Func<LogMessage, ConsoleColor> Extra
        {
            get => this[nameof(Extra)];
            set => this[nameof(Extra)] = value;
        }

        public event EventHandler Updated;

        public TokenConsoleColorStore()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            InitColors();
        }

        public new virtual ITokenConsoleColorStore Add(string key, Func<LogMessage, ConsoleColor> val)
        {
            base.Add(key, val);

            OnUpdated();

            return this;
        }

        public virtual Func<LogMessage, ConsoleColor> this[string key, Func<LogMessage, ConsoleColor> defaultFactory = null]
        {
            get => (ContainsKey(key) ? base[key] : (defaultFactory));
            set { base[key] = value; OnUpdated(); }
        }

        public new virtual Func<LogMessage, ConsoleColor> this[string key]
        {
            get => (ContainsKey(key) ? base[key] : (null));
            set { base[key] = value; OnUpdated(); }
        }

        public virtual ConsoleColor this[string key, LogMessage log]
        {
            get => (ContainsKey(key) ? base[key] : (m => ConsoleColor.White))(log);
        }

        protected virtual void InitColors()
        {
            base[nameof(Timestamp)] = DefaultTimestampColor;
            base[nameof(Message)] = DefaultMessageColor;
            base[nameof(Level)] = DefaultLevelTypeColor;
            base[nameof(Exception)] = DefaultExceptionColor;
            base[nameof(Extra)] = DefaultExtraColor;
        }

        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual ConsoleColor DefaultTimestampColor(LogMessage log)
        {
            return ConsoleColor.White;
        }

        protected virtual ConsoleColor DefaultMessageColor(LogMessage log)
        {
            return ConsoleColor.White;
        }

        protected virtual ConsoleColor DefaultLevelTypeColor(LogMessage log)
        {
            var color = ConsoleColor.White;

            switch (log.Level)
            {
                case LogLevel.Debug:
                    color = ConsoleColor.Gray;
                    break;

                case LogLevel.Info:
                    color = ConsoleColor.White;
                    break;

                case LogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;

                case LogLevel.Error:
                    color = ConsoleColor.Red;
                    break;

                case LogLevel.Critical:
                    color = ConsoleColor.Magenta;
                    break;
            }

            return color;
        }

        protected virtual ConsoleColor DefaultExceptionColor(LogMessage log)
        {
            return ConsoleColor.Gray;
        }

        protected virtual ConsoleColor DefaultExtraColor(LogMessage log)
        {
            return ConsoleColor.Cyan;
        }
    }
}