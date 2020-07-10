using SHAutomation.Core.Conditions;
using SHAutomation.Core.Enums;
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
            bool regenerateXPath = false;

            Diagnostics.Time(() =>
            {
                var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
                var condition_string = condition.ToString();
                _loggingService.Info(string.Format("Find called {0}", condition_string));
                bool canUseXpath = !(condition_string.Contains("OR") || condition_string.Contains("NOT"));
                List<(PropertyCondition Value, bool Ignore)> propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();
                if (parent == null)
                {
                    if (canUseXpath)
                    {
                        propertyConditions = GetPropertyConditions(condition);
                    }

                    control = GetXPathFromPropertyConditions(propertyConditions);

                    if (control == null)
                    {
                        regenerateXPath = true;
                        control = FindFirstDescendant(conditionFunc, timeout: timeout);
                    }

                }
                else
                {
                    control = parent.FindFirstDescendant(conditionFunc);
                }

                if (control == null)
                {
                    _loggingService.Error(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));

                    throw new ElementNotFoundException(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));
                }

                _loggingService.Info("Find found control", LoggingLevel.High);

                SHSpinWait.SpinUntil(() => control.SupportsOnscreen, 500);
                if (control.SupportsOnscreen)
                {
                    SHSpinWait.SpinUntil(() => control.IsOnscreen, offscreenTimeout);
                    _loggingService.Info("Find OnScreen: " + control.IsOnscreen);

                }
                else
                    _loggingService.Info("Find OnScreen is not supported", LoggingLevel.High);


                SaveXPathFromControl((SHAutomationElement)control, propertyConditions, regenerateXPath);
            }, _loggingService);

            return control;

        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, int timeout = 500, int offscreenTimeout = 500, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            bool exists = true;
            bool regenerateXPath = false;

            Diagnostics.Time(() =>
            {
                ISHAutomationElement control = null;

                if (parent == null)
                {
                    control = GetXPathElementFromCondition(conditionFunc, timeout);
                    if (control == null && !xpathOnly)
                    {
                        regenerateXPath = true;
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
                    exists = false;
                else if (checkOnScreen)
                {
                    control.WaitUntilPropertyEquals(PropertyId.Register(AutomationType.UIA3, 30022, "IsOffscreen"), false, offscreenTimeout);
                   
                    exists = control.IsOnscreen;
                }

                if (parent == null)
                    SaveXPathFromControl((SHAutomationElement)control, conditionFunc, regenerateXPath);

            }, _loggingService);

            return exists;
        }
    }
}
