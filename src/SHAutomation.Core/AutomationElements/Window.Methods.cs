using StreetsHeaver.SHAutomation.Core.Conditions;
using StreetsHeaver.SHAutomation.Core.Exceptions;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StreetsHeaver.SHAutomation.Core.AutomationElements
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

            _loggingService.Info($"Find called {conditionString}");

            bool canUseXpath = !(conditionString.Contains("OR") || conditionString.Contains("NOT"));
            List<(PropertyCondition Value, bool Ignore)> propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();

            if (parent == null)
            {
                if (canUseXpath)
                {
                    propertyConditions = GetPropertyConditions(condition);
                }

                var xpath = GetXPathValues(propertyConditions);

                if (xpath != null && xpath.Any())
                {
                    using (var cts = new CancellationTokenSource())
                    {

                        Task<ISHAutomationElement> getXPathTask = null;
                        Task<ISHAutomationElement> findTask = null;

                        try
                        {
                            var token = cts.Token;
                            getXPathTask = Task.Run(() => GetXPathFromValues(xpath, timeout, ref token), token);
                            findTask = Task.Run(() => FindFirstDescendant(condition, timeout, true, ref token), token);
                            Task.WaitAny(getXPathTask, findTask);
                            cts.Cancel();
                            control = getXPathTask.Result ?? findTask.Result;
                        }
                        catch (AggregateException ex)
                        {
                            if (ex.InnerException is TaskCanceledException)
                            {
                                if (getXPathTask.Status == TaskStatus.RanToCompletion)
                                    control = getXPathTask.Result;
                                else
                                    control = findTask.Result;
                            }

                        }


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
                if (parent.GetCurrentPropertyValue(PropertyId.Register(30022, "IsOffscreen")) != null)
                {
                    control = parent.FindFirstDescendant(conditionFunc, timeout);
                }
                else
                {
                    throw new ElementNotFoundException("Parent not found");
                }
            }

            if (control == null)
            {
                var errorMessage = $"Failed to find control by: {condition}";
                _loggingService.Error(errorMessage);
                throw new ElementNotFoundException(errorMessage);
            }

            _loggingService.Info(control.FoundByXPath ? "Find found control via XPath" : "Find found control via Condition");
            SaveXPathFromControl((SHAutomationElement)control, propertyConditions, regenerateXPath);
            return control;
        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            return Exists(conditionFunc, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500), checkOnScreen, parent, xpathOnly);
        }

        public bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, TimeSpan? offscreenTimeout = null, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false)
        {
            bool exists = false;
            bool regenerateXPath = false;
            ISHAutomationElement control = null;

            return Wait.UntilTrue(() =>
            {

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
                        catch (ElementNotFoundException)
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
                    if (offscreenTimeout.HasValue)
                        control.WaitUntilPropertyEquals(PropertyId.Register(30022, "IsOffscreen"), false, offscreenTimeout.Value);
                    else
                        control.WaitUntilPropertyEquals(PropertyId.Register(30022, "IsOffscreen"), false);

                    exists = control.IsOnscreen;
                }

                if (parent == null)
                    SaveXPathFromControl((SHAutomationElement)control, conditionFunc, regenerateXPath);

                return exists;
            }, timeout);
        }
    }
}
