using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestEssentials.Xunit.Sdk
{
    public class TestEssentialsTestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        public TestEssentialsTestFrameworkExecutor(AssemblyName assemblyName, 
            ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        { }

        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, 
            IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = new TestEssentialsTestAssemblyRunner(TestAssembly,
                testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
            {
                await assemblyRunner.RunAsync();
            }
        }
    }
}
