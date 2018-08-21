using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public class TokenConsoleColorStore
        : Dictionary<string, Func<LogMessage, ConsoleColor>>, ITokenConsoleColorStore
    {
        public Func<LogMessage, ConsoleColor> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public Func<LogMessage, ConsoleColor> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public Func<LogMessage, ConsoleColor> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public Func<LogMessage, ConsoleColor> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public Func<LogMessage, ConsoleColor> Extra
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

        ~TokenConsoleColorStore()
        {
            Updated = null;
        }

        public new ITokenConsoleColorStore Add(string key, Func<LogMessage, ConsoleColor> val)
        {
            base.Add(key, val);

            OnUpdated();

            return this;
        }

        public Func<LogMessage, ConsoleColor> this[string key, Func<LogMessage, ConsoleColor> defaultFactory = null]
        {
            get => (ContainsKey(key) ? base[key] : (defaultFactory));
            set { base[key] = value; OnUpdated(); }
        }

        public new Func<LogMessage, ConsoleColor> this[string key]
        {
            get => (ContainsKey(key) ? base[key] : (null));
            set { base[key] = value; OnUpdated(); }
        }

        public ConsoleColor this[string key, LogMessage log]
        {
            get => (ContainsKey(key) ? base[key] : (m => ConsoleColor.White))(log);
        }

        private void InitColors()
        {
            base[nameof(Timestamp)] = DefaultTimestampColor;
            base[nameof(Message)] = DefaultMessageColor;
            base[nameof(Level)] = DefaultLevelTypeColor;
            base[nameof(Exception)] = DefaultExceptionColor;
            base[nameof(Extra)] = DefaultExtraColor;
        }

        private void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        private ConsoleColor DefaultTimestampColor(LogMessage log)
        {
            return ConsoleColor.White;
        }

        private ConsoleColor DefaultMessageColor(LogMessage log)
        {
            return ConsoleColor.White;
        }

        private ConsoleColor DefaultLevelTypeColor(LogMessage log)
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

        private ConsoleColor DefaultExceptionColor(LogMessage log)
        {
            return ConsoleColor.Gray;
        }

        private ConsoleColor DefaultExtraColor(LogMessage log)
        {
            return ConsoleColor.Cyan;
        }
    }
}