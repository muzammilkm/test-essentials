using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using TestEssentials.ToolKit.Database.SqlServer.Options;

namespace TestEssentials.ToolKit.Database.SqlServer
{
    public class SqlServerTestDatabase : ITestDatabase, IDisposable
    {
        private Action<SqlServerDatabaseOptions> _configurationDatabaseOptions;
        private readonly SqlServerDatabaseOptions _databaseOptions;

        protected readonly SqlConnectionStringBuilder _dbBuilder;
        protected readonly SqlConnectionStringBuilder _masterDbBuilder;

        #region ctor
        public SqlServerTestDatabase(string connectionString)
        {
            _dbBuilder = new SqlConnectionStringBuilder(connectionString);
            _masterDbBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };
            _databaseOptions = new SqlServerDatabaseOptions();
        }
        #endregion

        protected void Drop()
        {
            if (!_databaseOptions.AlwayDrop)
                return;

            using (var connection = new SqlConnection(_masterDbBuilder.ConnectionString))
            {
                connection.Open();
                var cmdStr = $"IF DB_ID('{_dbBuilder.InitialCatalog}') IS NOT NULL " +
                            "BEGIN " +
                                $"ALTER DATABASE [{_dbBuilder.InitialCatalog}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                                $"DROP DATABASE [{_dbBuilder.InitialCatalog}]; " +
                            "END";
                using (var command = new SqlCommand(cmdStr, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        protected bool CheckDatabaseExists()
        {
            using (var connection = new SqlConnection(_masterDbBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT db_id('{_dbBuilder.InitialCatalog}')", connection))
                {
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public ITestDatabase ConfigureOptions(Action<SqlServerDatabaseOptions> configureDatabaseOptions)
        {
            _configurationDatabaseOptions += configureDatabaseOptions;
            return this;
        }

        public virtual ITestDatabase Build()
        {
            _configurationDatabaseOptions?.Invoke(_databaseOptions);

            if (!_databaseOptions.AlwayCreate && CheckDatabaseExists())
                return this;

            Drop();

            return this;
        }

        /// <summary>
        /// Reads the SQL scripts.
        /// </summary>
        /// <param name="folderPath">The folder containing sql files.</param>
        /// <param name="includeSubFolders">Seach in sub folders.</param>
        /// <returns></returns>
        public virtual ITestDatabase RunScriptFolder(string folderPath, bool includeSubFolders = true)
        {
            var scripts = new StringBuilder();
            var dir = new DirectoryInfo(folderPath);
            var searchOption = includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var file in dir.EnumerateFiles("*.sql", searchOption))
            {
                scripts.Append(File.ReadAllText(file.FullName));
            }
            return RunScript(scripts.ToString());
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <param name="filename">The script filename.</param>
        /// <returns></returns>
        public virtual ITestDatabase RunScriptFile(string filename)
        {
            return RunScript(File.ReadAllText(filename));
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        /// <returns></returns>
        public virtual ITestDatabase RunScript(string scripts)
        {
            using (var connection = new SqlConnection(_dbBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(scripts, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            return this;
        }

        public virtual void Dispose()
        {
            Drop();
        }
    }
}
