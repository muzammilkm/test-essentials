using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestEssentials.Xunit.Sdk
{
    public class TestEssentialsTestFramework : XunitTestFramework
    {
        public TestEssentialsTestFramework(IMessageSink messageSink)
            : base(messageSink)
        { }

        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
            => new TestEssentialsTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }
}
