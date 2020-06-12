using System.Collections.Generic;

namespace SHAutomation.Core.Conditions
{
    public class ConditionList : List<ConditionBase>
    {
        public ConditionList(params ConditionBase[] conditions)
        {
            AddRange(conditions);
        }
    }
}
