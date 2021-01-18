﻿using SHAutomation.Core.Conditions;
using SHAutomation.Core.Enums;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;

namespace SHAutomation.Core.AutomationElements
{
    public partial class Window
    {
        public ISHAutomationElement Find(string automationID, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10), parent);
        }

        public ISHAutomationElement Find(string automationID, TimeSpan timeout, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), timeout, TimeSpan.FromSeconds(10), parent);
        }

        public ISHAutomationElement Find(string automationID, TimeSpan timeout, TimeSpan offscreenTimeout, ISHAutomationElement parent = null)
        {
            return Find(x => x.ByAutomationId(automationID), timeout, offscreenTimeout, parent);
        }
        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, ISHAutomationElement parent = null)
        {
            return Find(conditionFunc, timeout, TimeSpan.FromSeconds(10), parent);

        }

        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, ISHAutomationElement parent = null)
        {
            return Find(conditionFunc, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10), parent);
        }

        public ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, TimeSpan offscreenTimeout, ISHAutomationElement parent = null)
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

                control = GetXPathFromPropertyConditions(propertyConditions);

                if (control == null)
                {
                    regenerateXPath = true;
                    control = FindFirstDescendant(conditionFunc, timeout: timeout);
                }

            }
            else
            {
                control = parent.FindFirstDescendant(conditionFunc, timeout);
            }

            if (control == null)
            {
                _loggingService.Error(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));

                throw new ElementNotFoundException(string.Format("Failed to find control by: {0}", conditionFunc(new ConditionFactory(Automation.PropertyLibrary)).ToString()));
            }

            _loggingService.Info("Find found control");

            SHSpinWait.SpinUntil(() => control.SupportsOnscreen, TimeSpan.FromMilliseconds(500));
            if (control.SupportsOnscreen)
            {
                SHSpinWait.SpinUntil(() => control.IsOnscreen, offscreenTimeout);
                _loggingService.Info("Find OnScreen: " + control.IsOnscreen);

            }
            else
                _loggingService.Info("Find OnScreen is not supported");


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

            return exists;
        }
    }
}
