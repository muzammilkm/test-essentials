using System;
using System.Collections.Generic;
using System.Text;

namespace TestEssentials.ToolKit.Database
{
    public class SqlServerDatabaseOptions : ITestDatabaseOptions
    {
        public bool WatchScript { get; set; }

        public bool AlwayCreate { get; set; }

        public bool AlwayDrop { get; set; }
    }
}
