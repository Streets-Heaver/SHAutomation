using SHAutomation.Core.Conditions;
using SHAutomation.Core.Enums;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SHAutomation.Core.AutomationElements
{
    public partial class Window
    {
        public ISHAutomationElement Find(string automationID, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), TimeSpan.FromSeconds(20), parent);
        }

        public ISHAutomationElement Find(string automationID, TimeSpan timeout, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), timeout, parent);
        }

        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, ISHAutomationElement parent = null)
        {
            return Find(conditionFunc, TimeSpan.FromSeconds(20), parent);
        }

        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, ISHAutomationElement parent = null)
        {
            ISHAutomationElement control = null;
            bool regenerateXPath = false;

            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            var conditionString = condition.ToString();

            _loggingService.Info(string.Format("Find called {0}", conditionString));
            bool canUseXpath = !(conditionString.Contains("OR") || conditionString.Contains("NOT"));
            List<(PropertyCondition Value, bool Ignore)> propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();
            if (parent == null)
            {
                if (canUseXpath)
                {
                    propertyConditions = GetPropertyConditions(condition);
                }
                condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
               
              
                var xpath = GetXPathValues(propertyConditions);
                if (xpath != null && xpath.Any())
                {
                    using (var cts = new CancellationTokenSource())
                    {
                        var token = cts.Token;
                        Task<ISHAutomationElement>[] tasks = {
                            Task.Run(() => GetXPathFromValues(xpath,timeout,ref token),token),
                            Task.Run(() => FindFirstDescendant(condition, timeout,true, ref token),token)
                        };
                        Task.WaitAny(tasks);
                        cts.Cancel();
                        control = tasks.FirstOrDefault(x => x.Status == TaskStatus.RanToCompletion && x.Result != null)?.Result;
                    }
                }
                else
                {
                    control = FindFirstDescendant(condition, timeout, true);
                }
                if (control != null)
                {
                    regenerateXPath = !control.FoundByXPath;
                }
            }
            else
            {
                if (parent.GetCurrentPropertyValue(PropertyId.Register(AutomationType.UIA3, 30022, "IsOffscreen")) != null)
                    control = parent.FindFirstDescendant(conditionFunc, timeout);
                else
                    throw new ElementNotFoundException("Parent not found");
            }

            if (control == null)
            {
                _loggingService.Error(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));

                throw new ElementNotFoundException(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));
            }
            _loggingService.Info("Find found control");

            SaveXPathFromControl((SHAutomationElement)control, propertyConditions, regenerateXPath);
            return control;
        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            return Exists(conditionFunc, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500), checkOnScreen, parent, xpathOnly);
        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, TimeSpan offscreenTimeout, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            bool exists = true;
            bool regenerateXPath = false;
            ISHAutomationElement control = null;

            if (parent == null)
            {
                if (xpathOnly)
                {
                    control = GetXPathElementFromCondition(conditionFunc, timeout);
                }
                else
                {
                    try
                    {
                        return Find(conditionFunc, timeout) != null;
                    }
                    catch(ElementNotFoundException)
                    {
                        return false;
                    }
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

            return exists;
        }
    }
}
