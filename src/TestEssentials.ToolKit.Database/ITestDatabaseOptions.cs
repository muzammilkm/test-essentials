namespace TestEssentials.ToolKit.Database
{
    public interface ITestDatabaseOptions
    {
        /// <summary>
        /// Determine to watch all sciprt changes &amp; reexecute in the same order. 
        /// </summary>
        bool WatchScript { get; set; }

        /// <summary>
        /// Determine to always create database even if it exists.
        /// </summary>
        bool AlwayCreate { get; set; }

        /// <summary>
        /// Determine to always drop the database.
        /// </summary>
        bool AlwayDrop { get; set; }
    }
}
