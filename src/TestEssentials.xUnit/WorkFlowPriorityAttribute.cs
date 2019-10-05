using System;

namespace TestEssentials.Xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WorkFlowPriorityAttribute : Attribute
    {
        public WorkFlowPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
