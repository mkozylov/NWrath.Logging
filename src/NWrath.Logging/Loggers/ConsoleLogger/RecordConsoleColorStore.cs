using System;
using System.Collections.Generic;

namespace NWrath.Logging
{
    public class RecordConsoleColorStore
        : Dictionary<string, Func<LogRecord, ConsoleColor>>
    {
        public Func<LogRecord, ConsoleColor> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public Func<LogRecord, ConsoleColor> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public Func<LogRecord, ConsoleColor> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public Func<LogRecord, ConsoleColor> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public Func<LogRecord, ConsoleColor> Extra
        {
            get => this[nameof(Extra)];
            set => this[nameof(Extra)] = value;
        }

        public event EventHandler Updated;

        public RecordConsoleColorStore()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            InitColors();
        }

        ~RecordConsoleColorStore()
        {
            Updated = null;
        }

        public new RecordConsoleColorStore Add(string key, Func<LogRecord, ConsoleColor> val)
        {
            base.Add(key, val);

            OnUpdated();

            return this;
        }

        public Func<LogRecord, ConsoleColor> this[string key, Func<LogRecord, ConsoleColor> defaultFactory = null]
        {
            get => (ContainsKey(key) ? base[key] : (defaultFactory));
            set { base[key] = value; OnUpdated(); }
        }

        public new Func<LogRecord, ConsoleColor> this[string key]
        {
            get => (ContainsKey(key) ? base[key] : (null));
            set { base[key] = value; OnUpdated(); }
        }

        public ConsoleColor this[string key, LogRecord record]
        {
            get => (ContainsKey(key) ? base[key] : (m => ConsoleColor.White))(record);
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

        private ConsoleColor DefaultTimestampColor(LogRecord record)
        {
            return ConsoleColor.White;
        }

        private ConsoleColor DefaultMessageColor(LogRecord record)
        {
            return ConsoleColor.White;
        }

        private ConsoleColor DefaultLevelTypeColor(LogRecord record)
        {
            var color = ConsoleColor.White;

            switch (record.Level)
            {
                case LogLevel.Debug:
                    color = ConsoleColor.Gray;
                    break;

                case LogLevel.Info:
                    color = ConsoleColor.Green;
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

        private ConsoleColor DefaultExceptionColor(LogRecord record)
        {
            return ConsoleColor.Gray;
        }

        private ConsoleColor DefaultExtraColor(LogRecord record)
        {
            return ConsoleColor.Cyan;
        }
    }
}