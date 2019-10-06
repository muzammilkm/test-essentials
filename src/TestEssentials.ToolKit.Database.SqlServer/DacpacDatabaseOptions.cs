using System.Collections.Generic;

namespace TestEssentials.ToolKit.Database.SqlServer
{
    public class DacpacDatabaseOptions: ITestDatabaseOptions
    {
        #region Ctor
        public DacpacDatabaseOptions()
        {
            DeployVariables = new Dictionary<string, string>();
        }
        #endregion

        public IDictionary<string, string> DeployVariables { get; set; }

        public string DacpacPath { get; set; }

        public bool AlwayCreate { get; set; }

        public bool AlwayDrop { get; set; }

        public bool AllowIncompatiblePlatform { get; set; }
    }
}
