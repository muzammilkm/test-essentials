using System;

namespace TestEssentials.ToolKit.Database
{
    public interface ITestDatabase
    {
        ITestDatabase Build();

        ITestDatabase RunScriptFolder(string folderPath, bool includeSubFolders = true);

        ITestDatabase RunScriptFile(string filename);

        ITestDatabase RunScript(string scripts);

        void Dispose();
    }
}
