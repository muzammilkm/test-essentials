namespace TestEssentials.ToolKit.Database
{
    public interface ITestDatabaseOptions
    {
        bool AlwayCreate { get; set; }

        bool AlwayDrop { get; set; }
    }
}
