using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace TestEssentials.ToolKit.Database
{
    public abstract class TestDatabase<TOptions> : ITestDatabase, IDisposable where TOptions : ITestDatabaseOptions, new()
    {
        private readonly IList<string> _scriptFiles;
        private readonly IList<FileSystemWatcher> _scriptFileWatchers;

        protected readonly TOptions _databaseOptions;
        protected readonly SqlConnectionStringBuilder _dbBuilder;
        protected readonly SqlConnectionStringBuilder _masterDbBuilder;


        #region ctor
        public TestDatabase(string connectionString)
        {
            _dbBuilder = new SqlConnectionStringBuilder(connectionString);
            _masterDbBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };
            _databaseOptions = new TOptions();
            _scriptFileWatchers = new List<FileSystemWatcher>();
            _scriptFiles = new List<string>();
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

        private void RunAllScriptsAgain(object source, FileSystemEventArgs e)
        {
            foreach (var filename in _scriptFiles)
            {
                RunScript(File.ReadAllText(filename));
            }
        }
        
        public abstract ITestDatabase Build();

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
            if (_databaseOptions.WatchScript)
            {
                _scriptFiles.Add(filename);
                var watcher = new FileSystemWatcher(Path.GetDirectoryName(filename))
                {
                    Filter = Path.GetFileName(filename),
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.LastAccess,
                    EnableRaisingEvents = true
                };
                watcher.Changed += RunAllScriptsAgain;
                _scriptFileWatchers.Add(watcher);
            }

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
