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
    public class TestEssentialsTestCollectionRunner : XunitTestCollectionRunner
    {
        private readonly IDictionary<Type, object> _assemblyFixtureMappings;
        private readonly IEnumerable<IXunitTestCase> _testCases;

        public TestEssentialsTestCollectionRunner(IDictionary<Type, object> assemblyFixtureMappings,
                                                    ITestCollection testCollection,
                                                    IEnumerable<IXunitTestCase> testCases,
                                                    IMessageSink diagnosticMessageSink,
                                                    IMessageBus messageBus,
                                                    ITestCaseOrderer testCaseOrderer,
                                                    ExceptionAggregator aggregator,
                                                    CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            _assemblyFixtureMappings = assemblyFixtureMappings;
            _testCases = testCases;
        }

        protected override async Task AfterTestCollectionStartingAsync()
        {
            await base.AfterTestCollectionStartingAsync();
            foreach (var mapping in _assemblyFixtureMappings)
            {
                CollectionFixtureMappings.Add(mapping.Key, mapping.Value);
            }
        }

        protected override Task BeforeTestCollectionFinishedAsync()
        {
            // We need to remove the assembly fixtures so they won't get disposed.
            foreach (var mapping in _assemblyFixtureMappings)
            {
                CollectionFixtureMappings.Remove(mapping.Key);
            }
            return base.BeforeTestCollectionFinishedAsync();
        }

        protected override void CreateCollectionFixture(Type fixtureType)
        {
            var ctors = fixtureType.GetTypeInfo()
                .DeclaredConstructors
                .Where(ci => !ci.IsStatic && ci.IsPublic)
                .ToList();

            if (ctors.Count != 1)
            {
                Aggregator.Add(new TestClassException($"Collection fixture type '{fixtureType.FullName}' may only define a single public constructor."));
                return;
            }

            var ctor = ctors[0];
            var missingParameters = new List<ParameterInfo>();
            var ctorArgs = ctor.GetParameters().Select(p =>
            {
                object arg = null;
                if (p.ParameterType == typeof(IMessageSink))
                    arg = DiagnosticMessageSink;
                else if (!_assemblyFixtureMappings.TryGetValue(p.ParameterType, out arg))
                    missingParameters.Add(p);
                return arg;
            }).ToArray();

            if (missingParameters.Count > 0)
                Aggregator.Add(new TestClassException(
                    $"Collection fixture type '{fixtureType.FullName}' had one or more unresolved constructor arguments: {string.Join(", ", missingParameters.Select(p => $"{p.ParameterType.Name} {p.Name}"))}"
                ));
            else
            {
                Aggregator.Run(() => CollectionFixtureMappings[fixtureType] = ctor.Invoke(ctorArgs));
            }
        }

        protected async override Task<RunSummary> RunTestClassesAsync()
        {
            var groups = TestCases
                .GroupBy(tc => tc.TestMethod.TestClass, TestClassComparer.Instance);
            try
            {
                if (TestCaseOrderer is ITestClassOrderer orderer)
                    groups = orderer.OrderTestClasses(groups);
            }
            catch (Exception ex)
            {
                if ((ex is TargetInvocationException tiex))
                  ex = ex.InnerException;
                DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Test class orderer '{TestCaseOrderer.GetType().FullName}' threw '{ex.GetType().FullName}' during ordering: {ex.Message}{Environment.NewLine}{ex.StackTrace}"));
            }
            var summary = new RunSummary();

            foreach (IGrouping<ITestClass, IXunitTestCase> testCasesByClass in groups)
            {
                summary.Aggregate(
                    await RunTestClassAsync(
                        testCasesByClass.Key,
                        (IReflectionTypeInfo)testCasesByClass.Key.Class,
                        testCasesByClass));

                if (CancellationTokenSource.IsCancellationRequested)
                    break;
            }

            return summary;
        }

        protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
        {
            var runner = new XunitTestClassRunner(
                testClass,
                @class,
                testCases,
                DiagnosticMessageSink,
                MessageBus,
                TestCaseOrderer,
                new ExceptionAggregator(Aggregator),
                CancellationTokenSource,
                CollectionFixtureMappings);
            return runner.RunAsync();
        }
    }
}
