using System;
using System.Collections.Generic;
using System.Linq;
using TestEssentials.Xunit.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace TestEssentials.Xunit.Orderer
{
    public class WorkFlowPriorityCollectionOrderer : ITestCollectionOrderer
    {
        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            var testCollectionList = new List<KeyValuePair<int, ITestCollection>>();
            foreach (var collection in testCollections)
            {
                var priority = 0;
                var type = collection.CollectionDefinition ??
                    collection.TestAssembly.Assembly.GetType(collection.DisplayName);
                var attibute = type.GetCustomAttributes(typeof(WorkFlowPriorityAttribute)).FirstOrDefault();
                if (attibute != null)
                    priority = attibute.GetNamedArgument<int>("Priority");
                testCollectionList.Add(new KeyValuePair<int, ITestCollection>(priority, collection));
            }

            return testCollectionList.OrderBy(x => x.Key).Select(x => x.Value);
        }

    }
}
