using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NWrath.Logging
{
    public class RecordFormatStore
        : IDictionary<string, Func<LogRecord, string>>
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

        public ICollection<string> Keys => _store.Keys;

        public ICollection<Func<LogRecord, string>> Values => _store.Values;

        public int Count => _store.Count;

        public bool IsReadOnly => false;

        public event EventHandler Updated;

        private Dictionary<string, Func<LogRecord, string>> _store = new Dictionary<string, Func<LogRecord, string>>(StringComparer.OrdinalIgnoreCase);

        public RecordFormatStore()
        {
            InitFormats();
        }

        ~RecordFormatStore()
        {
            Updated = null;
        }

        public RecordFormatStore Add(string key, Func<LogRecord, string> val)
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

        public bool TryGetValue(string key, out Func<LogRecord, string> value)
        {
            return _store.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Func<LogRecord, string>> item)
        {
            _store.Add(item.Key, item.Value);

            OnUpdated();
        }

        public void Clear()
        {
            _store.Clear();

            OnUpdated();
        }

        public bool Contains(KeyValuePair<string, Func<LogRecord, string>> item)
        {
            return _store.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Func<LogRecord, string>>[] array, int arrayIndex)
        {
            _store.CastTo<IDictionary<string, Func<LogRecord, string>>>()
                  .CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Func<LogRecord, string>> item)
        {
            var deleted = _store.Remove(item.Key);

            OnUpdated();

            return deleted;
        }

        public IEnumerator<KeyValuePair<string, Func<LogRecord, string>>> GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        public Func<LogRecord, string> this[string key, Func<LogRecord, string> defaultFactory = null]
        {
            get => (ContainsKey(key) ? _store[key] : (defaultFactory));
            set { _store[key] = value; OnUpdated(); }
        }

        public Func<LogRecord, string> this[string key]
        {
            get => (ContainsKey(key) ? _store[key] : (null));
            set { _store[key] = value; OnUpdated(); }
        }

        public string this[string key, LogRecord record]
        {
            get => (ContainsKey(key) ? _store[key] : (m => key))(record);
        }

        void IDictionary<string, Func<LogRecord, string>>.Add(string key, Func<LogRecord, string> value)
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
    }
}