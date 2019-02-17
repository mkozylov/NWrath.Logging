using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NWrath.Logging.Test.Structs
{
    public class DbParameterCollectionStub
        : DbParameterCollection
    {
        public override int Count => _parameters.Count;

        public override object SyncRoot => new object();

        private List<DbParameter> _parameters = new List<DbParameter>();

        public override int Add(object value)
        {
            _parameters.Add((DbParameter)value);

            return _parameters.Count;
        }

        public override void AddRange(Array values)
        {
            _parameters.AddRange(values.Cast<DbParameter>());
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            return _parameters.Contains(value);
        }

        public override bool Contains(string value)
        {
            return _parameters.Any(x => x.ParameterName == value);
        }

        public override void CopyTo(Array array, int index)
        {
            _parameters.CopyTo((DbParameter[])array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            return _parameters.IndexOf((DbParameter)value);
        }

        public override int IndexOf(string parameterName)
        {
            return _parameters.FindIndex(x => x.ParameterName == parameterName);
        }

        public override void Insert(int index, object value)
        {
            _parameters.Insert(index, (DbParameter)value);
        }

        public override void Remove(object value)
        {
            _parameters.Remove((DbParameter)value);
        }

        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            var item = _parameters.FirstOrDefault(x => x.ParameterName == parameterName);

            if (item != null)
            {
                _parameters.Remove(item);
            }
        }

        protected override DbParameter GetParameter(int index)
        {
            return _parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return _parameters.FirstOrDefault(x => x.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            _parameters[index] = value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            value.ParameterName = parameterName;

            _parameters.Add(value);
        }
    }
}