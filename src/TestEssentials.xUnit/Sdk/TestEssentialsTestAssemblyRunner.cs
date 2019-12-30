using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestEssentials.Xunit.Sdk
{
    public class TestEssentialsTestAssemblyRunner : XunitTestAssemblyRunner
    {
        private readonly IDictionary<Type, object> _assemblyFixtureMappings;

        public TestEssentialsTestAssemblyRunner(ITestAssembly testAssembly,
                                                IEnumerable<IXunitTestCase> testCases,
                                                IMessageSink diagnosticMessageSink,
                                                IMessageSink executionMessageSink,
                                                ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
            _assemblyFixtureMappings = new Dictionary<Type, object>();
        }


        protected override async Task AfterTestAssemblyStartingAsync()
        {
            await base.AfterTestAssemblyStartingAsync();
            Aggregator.Run(() =>
            {
                var fixturesAttributes = TestCases
                   .SelectMany(tc =>
                   {
                       if (tc.TestMethod.TestClass.TestCollection.CollectionDefinition == null)
                       {
                           return ((IReflectionTypeInfo)tc.TestMethod.TestClass.Class)
                              .Type
                              .GetTypeInfo()
                              .ImplementedInterfaces;
                       }
                       else
                       {
                           return tc.TestMethod.TestClass
                                    .TestCollection.CollectionDefinition.Interfaces
                                    .Select(i => ((IReflectionTypeInfo)i).Type);
                       }
                   })
                   .Where(i => i.GetTypeInfo().IsGenericType
                              && i.GetGenericTypeDefinition() == typeof(IAssemblyFixture<>))
                   .Select(u => u.GenericTypeArguments.Single())
                   .Distinct();

                // Instantiate all the fixtures
                foreach (var fixtureAttribute in fixturesAttributes)
                {
                    var ctorWithDiagnostics = fixtureAttribute.GetConstructor(new[] { typeof(IMessageSink) });
                    if (ctorWithDiagnostics != null)
                        _assemblyFixtureMappings[fixtureAttribute] = Activator.CreateInstance(fixtureAttribute, DiagnosticMessageSink);
                    else
                        _assemblyFixtureMappings[fixtureAttribute] = Activator.CreateInstance(fixtureAttribute);
                }
            });
        }

        protected override Task BeforeTestAssemblyFinishedAsync()
        {
            // Make sure we clean up everybody who is disposable, and use Aggregator.Run to isolate Dispose failures
            foreach (var disposable in _assemblyFixtureMappings.Values.OfType<IDisposable>())
                Aggregator.Run(disposable.Dispose);

            return base.BeforeTestAssemblyFinishedAsync();
        }

        protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
                                                                   ITestCollection testCollection,
                                                                   IEnumerable<IXunitTestCase> testCases,
                                                                   CancellationTokenSource cancellationTokenSource)
            => new TestEssentialsTestCollectionRunner(_assemblyFixtureMappings,
                testCollection,
                testCases,
                DiagnosticMessageSink,
                messageBus,
                TestCaseOrderer,
                new ExceptionAggregator(Aggregator),
                cancellationTokenSource).RunAsync();
    }
}
