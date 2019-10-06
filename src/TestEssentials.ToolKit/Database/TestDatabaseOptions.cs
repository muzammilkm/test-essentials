using System;
using System.Collections.Generic;
using System.Text;

namespace TestEssentials.ToolKit.Database
{
    public class TestDatabaseOptions : ITestDatabaseOptions
    {
        public bool AlwayCreate { get; set; }

        public bool AlwayDrop { get; set; }
    }
}
