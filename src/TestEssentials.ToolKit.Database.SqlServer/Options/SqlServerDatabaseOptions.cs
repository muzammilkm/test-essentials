using System;
using System.Collections.Generic;
using System.Text;

namespace TestEssentials.ToolKit.Database.SqlServer.Options
{
    public class SqlServerDatabaseOptions : ITestDatabaseOptions
    {
        public bool AlwayCreate { get; set; }

        public bool AlwayDrop { get; set; }
    }
}
