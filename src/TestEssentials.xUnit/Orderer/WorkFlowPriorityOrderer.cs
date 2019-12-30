using System;
using System.Collections.Generic;
using System.Linq;
using TestEssentials.Xunit.Attributes;
using TestEssentials.Xunit.Sdk;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestEssentials.Xunit.Orderer
{
    public class WorkFlowPriorityOrderer : ITestCaseOrderer, ITestClassOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(x => GetCaseOrder(x));
        }

        public IEnumerable<IGrouping<ITestClass, IXunitTestCase>> OrderTestClasses(IEnumerable<IGrouping<ITestClass, IXunitTestCase>> testCaseGroups)
        {
            return testCaseGroups.OrderBy(g => GetClassOrder(g.Key));
        }

        private int GetCaseOrder(ITestCase testCase)
        {
            return GetPriority(testCase.TestMethod.Method.GetCustomAttributes(typeof(WorkFlowPriorityAttribute)));
        }

        private int GetClassOrder(ITestClass tc)
        {
            return GetPriority(tc.Class.GetCustomAttributes(typeof(WorkFlowPriorityAttribute)));
        }

        private int GetPriority(IEnumerable<IAttributeInfo> attributeInfos)
        {
            var attibute = attributeInfos?.FirstOrDefault();
            if (attibute == null)
                return 0;

            return attibute.GetNamedArgument<int>("Priority");
        }
    }
}
