using Newtonsoft.Json;
using SHAutomation.Core.Caching;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.ExtensionMethods;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

[assembly: InternalsVisibleTo("SHAutomation.Core.Tests")]
namespace SHAutomation.Core.AutomationElements
{
    public partial class Window
    {
        private SynchronizedCollection<string> _xPathValues = new SynchronizedCollection<string>();
        private bool _hasXPathValue = false;

        public IFileSystem FileSystem { get; set; } = new FileSystem();
        public ICacheService CacheService { get; set; }
        public SynchronizedCollection<(string identifier, string property, string xpath)> XPathList { get; private set; } = new SynchronizedCollection<(string identifier, string property, string xpath)>();
        internal SynchronizedCollection<string> UsedXPaths = new SynchronizedCollection<string>();

        private string _xpathGetContent;

        public void GetXPathCache(string testName)
        {

            var cacheKey = CacheService.GenerateCacheKey(testName);


            _loggingService.Info($"Attempting to find cache member with key {testName}");

            _xpathGetContent = CacheService.GetCacheValue(cacheKey, testName);


            if (!string.IsNullOrEmpty(_xpathGetContent))
            {
                _loggingService.Warn("XPath Json found in cache");

                XPathList = JsonConvert.DeserializeObject<SynchronizedCollection<(string identifier, string property, string xpath)>>(_xpathGetContent);
                if (XPathList == null)
                {
                    XPathList = new SynchronizedCollection<(string identifier, string property, string xpath)>();
                }
            }
            else
            {
                _loggingService.Warn("XPath Json not found in cache");

                XPathList = new SynchronizedCollection<(string identifier, string property, string xpath)>();
            }

        }

        public void SaveXPathCache(string testName)
        {
            var cacheKey = CacheService.GenerateCacheKey(testName);

            lock (XPathList.SyncRoot)
            {
                lock (UsedXPaths.SyncRoot)
                {
                    if (XPathList.Any())
                    {
                        var tempXPath = new List<(string identifier, string property, string xpath)>(XPathList.Distinct().Where(x => UsedXPaths.Contains(x.xpath)).ToList());

                        var output = JsonConvert.SerializeObject(tempXPath);
                        XPathList = new SynchronizedCollection<(string identifier, string property, string xpath)>(tempXPath);
                        //Only perform update when there are new xpaths to write
                        if (_xpathGetContent != output)
                        {
                            _loggingService.Info("Saving XPaths to cache");


                            CacheService.SetCacheValue(cacheKey, output);

                        }
                        else
                            _loggingService.Info("No XPath changes detected so not saving");

                    }
                }
            }
        }

        public ISHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            return GetXPathElementFromCondition(conditionFunc, TimeSpan.FromSeconds(10));
        }

        public ISHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            var condition_string = condition.ToString();
            _loggingService.Info(string.Format("GetXPathElementFromCondition called {0}", condition_string));

            bool canUseXpath = !(condition_string.Contains("OR") || condition_string.Contains("NOT"));
            return canUseXpath ? GetXPathFromPropertyConditions(GetPropertyConditions(condition), timeout) : null;
        }


        public List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(ConditionBase condition)
        {
            var propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();
            if (condition is AndCondition andCondition)
            {
                if (andCondition.Conditions.All(c => c is PropertyCondition))
                {
                    foreach (var prop in andCondition.Conditions)
                    {
                        var propCond = (PropertyCondition)prop;
                        propertyConditions.Add((propCond, propCond.PropertyConditionFlags == PropertyConditionFlag.IgnoreCase));
                    }
                }
            }
            else if (condition is PropertyCondition prop)
            {
                propertyConditions.Add((prop, prop.PropertyConditionFlags == PropertyConditionFlag.IgnoreCase));
            }
            return propertyConditions;
        }

        private List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return GetPropertyConditions(condition);
        }

        private ISHAutomationElement GetXPathFromPropertyConditions(List<(PropertyCondition Value, bool Ignore)> propertyList, TimeSpan xPathTimeout)
        {
            List<string> retrievedXPath = new List<string>();

            if (propertyList.Any())
            {
                if (propertyList.Count == 1)
                {
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(propertyList.First().Value.Value.ToString()), StringExtensions.ConvertStringPropertyToValidXPath(propertyList.First().Value.Property.Name), out retrievedXPath);
                    _xPathValues = new SynchronizedCollection<string>(retrievedXPath);

                }
                else
                {
                    List<string> propList = new List<string>();
                    foreach ((PropertyCondition Value, bool Ignore) p in propertyList)
                    {
                        propList.Add(StringExtensions.ConvertStringPropertyToValidXPath(p.Value.Property.Name.ToString()));

                    }
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(string.Join("`", propertyList.Select(x => x.Value.Value.ToString()))), string.Join("`", propList), out retrievedXPath);
                    _xPathValues = new SynchronizedCollection<string>(retrievedXPath);

                }
                if (retrievedXPath.Any())
                    return FindFirstByXPath(retrievedXPath, xPathTimeout);
                else
                {
                    return null;
                }

            }
            else
                return null;
        }
        private ISHAutomationElement GetXPathFromValues(List<string> values, TimeSpan timeout, ref CancellationToken token)
        {
            return FindFirstByXPath(values, timeout, ref token);
        }
        public List<string> GetXPathValues(List<(PropertyCondition Value, bool Ignore)> propertyList)
        {
            List<string> retrievedXPath = new List<string>();
            if (propertyList.Any())
            {
                if (propertyList.Count == 1)
                {
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(propertyList.First().Value.Value.ToString()), StringExtensions.ConvertStringPropertyToValidXPath(propertyList.First().Value.Property.Name), out retrievedXPath);
                    _xPathValues = new SynchronizedCollection<string>(retrievedXPath);
                    return retrievedXPath;
                }
                else
                {
                    List<string> propList = new List<string>();
                    foreach ((PropertyCondition Value, bool Ignore) p in propertyList)
                    {
                        propList.Add(StringExtensions.ConvertStringPropertyToValidXPath(p.Value.Property.Name.ToString()));

                    }
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(string.Join("`", propertyList.Select(x => x.Value.Value.ToString()))), string.Join("`", propList), out retrievedXPath);
                    _xPathValues = new SynchronizedCollection<string>(retrievedXPath);
                    return retrievedXPath;
                }

            }
            return retrievedXPath;
        }

        public void SaveXPathFromControl(SHAutomationElement control, Func<ConditionFactory, ConditionBase> conditionFunc, bool regenerateXPath)
        {
            SaveXPathFromControl(control, GetPropertyConditions(conditionFunc), regenerateXPath);
        }
        public void SaveXPathFromControl(SHAutomationElement control, List<(PropertyCondition Value, bool Ignore)> propertyList, bool regenerateXPath)
        {
            if (control?.FrameworkAutomationElement != null && propertyList.Any())
            {
                if (propertyList.Count > 1)
                {
                    List<string> values = new List<string>();
                    List<string> properties = new List<string>();

                    if (propertyList.Select(x => x.Value).OfType<PropertyCondition>().Count() != propertyList.Count)
                    {
                        return;
                    }

                    foreach ((PropertyCondition Value, bool Ignore) p in propertyList)
                    {
                        if (p.Ignore)
                        {
                            properties.Add(StringExtensions.ConvertStringPropertyToValidXPath(p.Value.Property.Name));
                            values.Add(StringExtensions.ConvertStringValueToValidXPath(control.GetType().GetProperty(p.Value.Property.Name).GetValue(control, null).ToString()));
                        }
                        else
                        {
                            properties.Add(StringExtensions.ConvertStringPropertyToValidXPath(p.Value.Property.Name));
                            values.Add(StringExtensions.ConvertStringValueToValidXPath(p.Value.Value.ToString()));
                        }
                    }
                    GenerateXPathAndCache(string.Join("`", values.ToArray()), string.Join("`", properties.ToArray()), control, regenerateXPath);
                }
                else
                {
                    var firstProp = propertyList.First();
                    string value = firstProp.Value.Value.ToString();
                    if (firstProp.Ignore)
                    {
                        value = control.GetType().GetProperty(firstProp.Value.Property.Name).GetValue(control, null).ToString();
                    }

                    GenerateXPathAndCache(value, firstProp.Value.Property.Name, control, regenerateXPath);


                }
            }
        }


        private void GenerateXPathAndCache(string identifier, string property, SHAutomationElement element, bool regenerateXPath)
        {
            lock (_xPathValues)
            {
                try
                {

                    if (!_xPathValues.Any() || regenerateXPath)
                    {
                        var xpathBuilder = new StringBuilder();
                        var xpathToElement = XPathHelper.GetXPathToElement(element, this);
                        if (!string.IsNullOrEmpty(xpathToElement))
                        {
                            xpathBuilder.Append(xpathToElement);
                            _xPathValues.Add(xpathBuilder.ToString());
                        }
                        _hasXPathValue = false;
                    }
                    else
                    {
                        _hasXPathValue = true;
                    }

                    if (_xPathValues.Any() && !_hasXPathValue)
                    {
                        foreach (var xpathValue in _xPathValues)
                        {
                            var items = xpathValue.Split('/').ToList();
                            // "" and Window
                            var lastItem = items.Last();
                            items.RemoveRange(0, 1);

                            lastItem = lastItem.Split('[')[0];

                            if (property.Contains("`"))
                            {
                                var splitAnds = identifier.Split(new string[] { "`" }, StringSplitOptions.None);
                                var splitProperties = property.Split(new string[] { "`" }, StringSplitOptions.None);
                                var stringBuilder = new StringBuilder();
                                stringBuilder.Append("[");
                                for (int i = 0; i < splitAnds.Length - 1; i++)
                                {
                                    stringBuilder.Append("@" + splitProperties[i] + " = '" + splitAnds[i] + "' and ");
                                }
                                lastItem = lastItem + stringBuilder.ToString() + string.Format("@{0} = '{1}']", splitProperties.Last(), splitAnds.Last());
                            }
                            else
                            {
                                lastItem = lastItem + string.Format("[@" + property + "='{0}']", identifier);
                            }

                            if (items.Any())
                            {
                                items[items.Count - 1] = lastItem;
                            }
                            var newStr = string.Join("/", items);

                            //If cache already contains the value then remove and replace with up-to-date version
                            var existingXPath = XPathList.SingleOrDefault(x => x.identifier == identifier && x.xpath == ("/" + newStr));
                            if (existingXPath.Equals(default(ValueTuple<string, string, string>)))
                            {
                                XPathList.Remove(existingXPath);
                            }

                            XPathList.Add((identifier, property, xpath: "/" + newStr));
                            UsedXPaths.Add("/" + newStr);

                        }
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.Error(string.Format("Issue creating Xpath: {0}", ex));

                    if (ex is ArgumentException)
                    {
                        _loggingService.Error("Attempted to add duplicate key to dictionary");

                    }

                    throw;

                }

            }
        }

        private bool GetXPathValueFromCache(string identifier, string property, out List<string> xPathValues)
        {
            _loggingService.Info($"Getting XPath From Cache: {identifier}");

            xPathValues = new List<string>();

            lock (XPathList.SyncRoot)
            {
                var xPathItems = XPathList.Where(x => x.identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase) && x.property == property);
                if (xPathItems.Any())
                {
                    xPathValues = xPathItems.Select(x => x.xpath).ToList();
                    _loggingService.Info("XPath found in Cache");
                    return true;
                }
                else
                {
                    _loggingService.Info("XPath not found");
                    return false;
                }
            }
        }

        public new ISHAutomationElement FindFirstByXPath(string xpath, TimeSpan spinWaitTimeout)
        {
            ISHAutomationElement element = null;
            if (xpath.Contains("&quot;"))
            {
                xpath = xpath.Replace("&quot;", "'");
            }

            if (spinWaitTimeout != TimeSpan.Zero)
            {
                Wait.UntilTrue(() =>
                {
                    element = FindFirstByXPath(xpath);
                    return element?.FrameworkAutomationElement != null;
                }, spinWaitTimeout);
            }
            else
            {
                element = FindFirstByXPath(xpath);
            }

            if (element == null)
            {
                _xPathValues = new SynchronizedCollection<string>();
            }
            else
            {
                Wait.UntilTrue(() => element?.FrameworkAutomationElement != null, TimeSpan.FromSeconds(5));
                element.FoundByXPath = true;
                return (SHAutomationElement)element;
            }

            return null;
        }

        private ISHAutomationElement FindFirstByXPath(List<string> xpath, TimeSpan spinWaitTimeout)
        {
            var token = CancellationToken.None;
            return FindFirstByXPath(xpath, spinWaitTimeout, ref token);
        }
        private ISHAutomationElement FindFirstByXPath(List<string> xpath, TimeSpan spinWaitTimeout, ref CancellationToken token)
        {
            ISHAutomationElement validXpath = null;

            Wait.UntilTrue(() => FoundXPathInList(xpath, out validXpath), spinWaitTimeout, token);
            if (validXpath == null)
            {
                _xPathValues = new SynchronizedCollection<string>();
            }
            if (validXpath?.FrameworkAutomationElement != null)
            {
                return validXpath;
            }
            return null;
        }
        private bool FoundXPathInList(List<string> xpaths, out ISHAutomationElement xPathObject)
        {
            xPathObject = null;
            bool found = false;
            foreach (var xpath in xpaths)
            {
                var tempXPathObject = FindFirstByXPath(xpath, TimeSpan.Zero);
                if (tempXPathObject?.FrameworkAutomationElement != null)
                {
                    found = true;
                    xPathObject = tempXPathObject;
                    UsedXPaths.Add(xpath);
                    break;
                }
            }

            return found;


        }
    }
}
