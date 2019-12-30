using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestEssentials.Xunit.Sdk
{
    public interface ITestClassOrderer
    {
        IEnumerable<IGrouping<ITestClass, IXunitTestCase>> OrderTestClasses(IEnumerable<IGrouping<ITestClass, IXunitTestCase>> testCaseGroups);
    }
}
