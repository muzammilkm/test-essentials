namespace TestEssentials.ToolKit.Database
{
    public class SqlServerDatabaseOptions : ITestDatabaseOptions
    {
        /// <summary>
        /// Determine to watch all sciprt changes &amp; reexecute in the same order. 
        /// </summary>
        public bool WatchScript { get; set; }

        /// <summary>
        /// Determine to always create database even if it exists.
        /// </summary>
        public bool AlwayCreate { get; set; }

        /// <summary>
        /// Determine to always drop the database.
        /// </summary>
        public bool AlwayDrop { get; set; }
    }
}
