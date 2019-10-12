using System.Collections.Generic;

namespace TestEssentials.ToolKit.Database.Dacpac
{
    public class DacpacDatabaseOptions: ITestDatabaseOptions
    {
        #region Ctor
        public DacpacDatabaseOptions()
        {
            DeployVariables = new Dictionary<string, string>();
        }
        #endregion

        /// <summary>
        /// List of Dacpac Variables
        /// </summary>
        public IDictionary<string, string> DeployVariables { get; set; }

        /// <summary>
        /// Dacpac path to publish the database.
        /// </summary>
        public string DacpacPath { get; set; }

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

        /// <summary>
        /// Determine to allow incompatible platform.
        /// </summary>
        public bool AllowIncompatiblePlatform { get; set; }
    }
}
