using System;
using System.Data;
using System.Data.Common;

namespace NWrath.Logging.Test.Structs
{
    public class DbConnectionStub
        : DbConnection
    {
        public override string ConnectionString { get; set; }

        public override string Database => _database;

        public override string DataSource => _dataSource;

        public override string ServerVersion => _serverVersion;

        public override ConnectionState State => _state;

        public virtual DbCommand LastCommand { get; private set; }

        private string _database;
        private string _dataSource;
        private string _serverVersion;
        private ConnectionState _state;

        public override void ChangeDatabase(string databaseName)
        {
            _database = databaseName;
        }

        public virtual void ChangeDataSource(string dataSource)
        {
            _dataSource = dataSource;
        }

        public virtual void ChangeServerVersion(string serverVersion)
        {
            _serverVersion = serverVersion;
        }

        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            LastCommand = new DbCommandStub { Connection = this };

            return LastCommand;
        }
    }
}