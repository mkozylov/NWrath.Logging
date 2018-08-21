using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using NWrath.Synergy.Common.Extensions;

namespace NWrath.Logging
{
    public class TokenFormatStore
        : ITokenFormatStore
    {
        public Func<LogMessage, string> Timestamp
        {
            get => this[nameof(Timestamp)];
            set => this[nameof(Timestamp)] = value;
        }

        public Func<LogMessage, string> Message
        {
            get => this[nameof(Message)];
            set => this[nameof(Message)] = value;
        }

        public Func<LogMessage, string> Level
        {
            get => this[nameof(Level)];
            set => this[nameof(Level)] = value;
        }

        public Func<LogMessage, string> Exception
        {
            get => this[nameof(Exception)];
            set => this[nameof(Exception)] = value;
        }

        public Func<LogMessage, string> Extra
        {
            get => this[nameof(Extra)];
            set => this[nameof(Extra)] = value;
        }

        public ICollection<string> Keys => _store.Keys;

        public ICollection<Func<LogMessage, string>> Values => _store.Values;

        public int Count => _store.Count;

        public bool IsReadOnly => false;

        public event EventHandler Updated;

        private Dictionary<string, Func<LogMessage, string>> _store = new Dictionary<string, Func<LogMessage, string>>(StringComparer.OrdinalIgnoreCase);

        public TokenFormatStore()
        {
            InitFormats();
        }

        ~TokenFormatStore()
        {
            Updated = null;
        }

        public ITokenFormatStore Add(string key, Func<LogMessage, string> val)
        {
            _store.Add(key, val);

            OnUpdated();

            return this;
        }

        public bool ContainsKey(string key)
        {
            return _store.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            var deleted = _store.Remove(key);

            OnUpdated();

            return deleted;
        }

        public bool TryGetValue(string key, out Func<LogMessage, string> value)
        {
            return _store.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Func<LogMessage, string>> item)
        {
            _store.Add(item.Key, item.Value);

            OnUpdated();
        }

        public void Clear()
        {
            _store.Clear();

            OnUpdated();
        }

        public bool Contains(KeyValuePair<string, Func<LogMessage, string>> item)
        {
            return _store.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Func<LogMessage, string>>[] array, int arrayIndex)
        {
            _store.CastTo<IDictionary<string, Func<LogMessage, string>>>()
                  .CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Func<LogMessage, string>> item)
        {
            var deleted = _store.Remove(item.Key);

            OnUpdated();

            return deleted;
        }

        public IEnumerator<KeyValuePair<string, Func<LogMessage, string>>> GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        public Func<LogMessage, string> this[string key, Func<LogMessage, string> defaultFactory = null]
        {
            get => (ContainsKey(key) ? _store[key] : (defaultFactory));
            set { _store[key] = value; OnUpdated(); }
        }

        public Func<LogMessage, string> this[string key]
        {
            get => (ContainsKey(key) ? _store[key] : (null));
            set { _store[key] = value; OnUpdated(); }
        }

        public string this[string key, LogMessage log]
        {
            get => (ContainsKey(key) ? _store[key] : (m => key))(log);
        }

        void IDictionary<string, Func<LogMessage, string>>.Add(string key, Func<LogMessage, string> value)
        {
            Add(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void InitFormats()
        {
            _store[nameof(Timestamp)] = DefaultTimestampFormat;
            _store[nameof(Message)] = DefaultMessageFormat;
            _store[nameof(Level)] = DefaultLevelTypeFormat;
            _store[nameof(Exception)] = DefaultExceptionFormat;
            _store[nameof(Extra)] = DefaultExtraFormat;
            _store["NewLine"] = m => Environment.NewLine;
            _store["ExNewLine"] = (m => m.Exception == null ? "" : Environment.NewLine);
        }

        private void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        private string DefaultTimestampFormat(LogMessage log)
        {
            return log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }

        private string DefaultMessageFormat(LogMessage log)
        {
            return log.Message;
        }

        private string DefaultLevelTypeFormat(LogMessage log)
        {
            return log.Level.ToString();
        }

        private string DefaultExceptionFormat(LogMessage log)
        {
            return log.Exception?.ToString() ?? "";
        }

        private string DefaultExtraFormat(LogMessage log)
        {
            return log.Extra.Count == 0
                ? string.Empty
                : log.Extra.AsJson();
        }
    }
}