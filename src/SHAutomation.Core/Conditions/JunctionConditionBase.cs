using System.Collections.Generic;

namespace StreetsHeaver.SHAutomation.Core.Conditions
{
    public abstract class JunctionConditionBase : ConditionBase
    {
        protected JunctionConditionBase()
        {
            Conditions = new List<ConditionBase>();
        }

        public List<ConditionBase> Conditions { get; }

        public int ChildCount => Conditions.Count;
    }
}
