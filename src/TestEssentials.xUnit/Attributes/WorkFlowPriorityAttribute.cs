using System;

namespace TestEssentials.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WorkFlowPriorityAttribute : PriorityAttribute
    {
        public WorkFlowPriorityAttribute(int priority)
            :base(priority)
        {
        }
    }
}
