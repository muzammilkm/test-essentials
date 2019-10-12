using System;

namespace TestEssentials.ToolKit.Database
{
    public class SqlServerTestDatabase : TestDatabase<SqlServerDatabaseOptions>
    {
        private Action<SqlServerDatabaseOptions> _configurationDatabaseOptions;
        
        #region ctor
        public SqlServerTestDatabase(string connectionString)
            :base(connectionString)
        {
        }
        #endregion

        public ITestDatabase ConfigureOptions(Action<SqlServerDatabaseOptions> configureDatabaseOptions)
        {
            _configurationDatabaseOptions += configureDatabaseOptions;
            return this;
        }

        public override ITestDatabase Build()
        {
            _configurationDatabaseOptions?.Invoke(_databaseOptions);

            if (!_databaseOptions.AlwayCreate && CheckDatabaseExists())
                return this;

            Drop();

            return this;
        }

    }
}
