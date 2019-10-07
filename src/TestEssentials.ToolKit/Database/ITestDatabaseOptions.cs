namespace TestEssentials.ToolKit.Database
{
    public interface ITestDatabaseOptions
    { 
        bool WatchScript { get; set; }

        bool AlwayCreate { get; set; }

        bool AlwayDrop { get; set; }
    }
}
