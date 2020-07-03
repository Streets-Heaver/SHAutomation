using SHAutomation.Core.Conditions;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAutomation.Core.AutomationElements
{
    public partial class Window
    {

        public ISHAutomationElement Find(string automationID, int timeout = 20000, int offscreenTimeout = 10000, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), timeout, offscreenTimeout, parent);
        }

        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, int offscreenTimeout = 10000, ISHAutomationElement parent = null)
        {
            ISHAutomationElement control = null;
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            var condition_string = condition.ToString();
            bool canUseXpath = !(condition_string.Contains("OR") || condition_string.Contains("NOT"));
            List<(PropertyCondition Value, bool Ignore)> propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();
            if (parent == null)
            {
                if (canUseXpath)
                {
                    propertyConditions = GetPropertyConditions(condition);
                }
                control = GetXpathFromPropertyConditions(propertyConditions) ?? FindFirstDescendant(conditionFunc, timeout: timeout);

            }
            else
            {
                control = parent.FindFirstDescendant(conditionFunc);
            }

            if (control == null)
            {
                throw new ElementNotFoundException(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));
            }

            SHSpinWait.SpinUntil(() => control.SupportsOnscreen, 500);
            if (control.SupportsOnscreen)
            {
                SHSpinWait.SpinUntil(() => control.IsOnscreen, offscreenTimeout);
            }

            SaveXPathFromControl((SHAutomationElement)control, propertyConditions);
            return control;

        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, int timeout = 500, int offscreenTimeout = 500, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            ISHAutomationElement control = null;

            if (parent == null)
            {
                control = GetXPathElementFromCondition(conditionFunc, timeout);
                if (control == null && !xpathOnly)
                {
                    control = FindFirstDescendant(conditionFunc, timeout: timeout);
                }
            }
            else
            {
                if (xpathOnly)
                {
                    throw new InvalidOperationException("Cannot search with xpath using parent");
                }
                control = parent.FindFirstDescendant(conditionFunc, timeout: timeout);
            }

            if (control == null)
                return false;
            else if (checkOnScreen)
            {
                control.WaitUntilPropertyEquals(PropertyId.Register(AutomationType.UIA3, 30022, "IsOffscreen"), false, offscreenTimeout);
                if (parent == null)
                {
                    SaveXPathFromControl((SHAutomationElement)control, conditionFunc);
                }
                return control.IsOnscreen;
            }
            if (parent == null)
            {
                SaveXPathFromControl((SHAutomationElement)control, conditionFunc);
            }
            return true;
        }
    }
}
