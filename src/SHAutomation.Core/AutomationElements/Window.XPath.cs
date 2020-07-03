using Newtonsoft.Json;
using SHAutomation.Core.Caching;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.ExtensionMethods;
using SHAutomation.Core.StaticClasses;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHAutomation.Core.AutomationElements
{
    public partial class Window
    {
        private List<string> _xPathValues = new List<string>();
        private bool _hasXPathValue = false;
        public List<(string identifier, string property, string xpath)> XPathList = new List<(string identifier, string property, string xpath)>();
        public IFileSystem FileSystem { get; set; } = new FileSystem();
        public ICacheService CacheService { get; set; } 
        private string _xpathGetContent;

        public void GetXPathCache(string testName)
        {

            var cacheKey = CacheService.GenerateCacheKey(testName);

            try
            {
                _xpathGetContent = CacheService.GetCacheValue(cacheKey, testName);
            }
            catch (RedisTimeoutException)
            {
                _xpathGetContent = CacheService.GetCacheValue(cacheKey, testName);
            }

            if (!string.IsNullOrEmpty(_xpathGetContent))
            {
                XPathList = JsonConvert.DeserializeObject<List<(string identifier, string property, string xpath)>>(_xpathGetContent);
                if (XPathList == null)
                {
                    XPathList = new List<(string identifier, string property, string xpath)>();
                }
            }
            else
            {
                XPathList = new List<(string identifier, string property, string xpath)>();
            }

        }

        public void SaveXPathCache(string testName)
        {

            var cacheKey = CacheService.GenerateCacheKey(testName);

            if (XPathList.Any())
            {
                var output = JsonConvert.SerializeObject(XPathList);

                //Only perform update when there are new xpaths to write
                if (_xpathGetContent != output)
                {
                    try
                    {
                        CacheService.SetCacheValue(cacheKey, output);
                    }
                    catch (RedisConnectionException)
                    {
                        CacheService.SetCacheValue(cacheKey, output);
                    }
                }

            }


        }


        public SHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 10000)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            var condition_string = condition.ToString();
            bool canUseXpath = !(condition_string.Contains("OR") || condition_string.Contains("NOT"));
            return canUseXpath ? GetXpathFromPropertyConditions(GetPropertyConditions(condition), timeout) : null;
        }


        public List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(ConditionBase condition)
        {
            List<(PropertyCondition Value, bool Ignore)> propertyConditions = new List<(PropertyCondition Value, bool Ignore)>();
            if (condition is AndCondition)
            {
                var andCondition = condition as AndCondition;
                if (andCondition.Conditions.OfType<PropertyCondition>().Count() != andCondition.Conditions.Count())
                {
                    return new List<(PropertyCondition Value, bool Ignore)>();
                }
                foreach (var prop in andCondition.Conditions)
                {
                    var propCond = prop as PropertyCondition;
                    propertyConditions.Add((propCond, propCond.PropertyConditionFlags == PropertyConditionFlags.IgnoreCase));
                }
            }
            else if (condition is PropertyCondition)
            {
                var prop = condition as PropertyCondition;
                propertyConditions.Add((prop, prop.PropertyConditionFlags == PropertyConditionFlags.IgnoreCase));
            }
            return propertyConditions.Any() ? propertyConditions : new List<(PropertyCondition Value, bool Ignore)>();
        }
        private List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return GetPropertyConditions(condition);
        }

        private SHAutomationElement GetXpathFromPropertyConditions(List<(PropertyCondition Value, bool Ignore)> propertyList, int xPathTimeout = 10000)
        {
            if (propertyList.Any())
            {
                if (propertyList.Count == 1)
                {
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(propertyList.First().Value.Value.ToString()), StringExtensions.ConvertStringPropertyToValidXPath(propertyList.First().Value.Property.Name), out _xPathValues);
                }
                else
                {
                    List<string> propList = new List<string>();
                    foreach ((PropertyCondition Value, bool Ignore) p in propertyList)
                    {
                        propList.Add(StringExtensions.ConvertStringPropertyToValidXPath(p.Value.Property.Name.ToString()));

                    }
                    GetXPathValueFromCache(StringExtensions.ConvertStringValueToValidXPath(string.Join("`", propertyList.Select(x => x.Value.Value.ToString()))), string.Join("`", propList), out _xPathValues);
                }
                return _xPathValues.Any() ? FindFirstByXPath(_xPathValues, xPathTimeout) : null;
            }
            else
                return null;
        }
        public void SaveXPathFromControl(SHAutomationElement control, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            SaveXPathFromControl(control, GetPropertyConditions(conditionFunc));
        }
        public void SaveXPathFromControl(SHAutomationElement control, List<(PropertyCondition Value, bool Ignore)> propertyList)
        {
            if (control?.FrameworkAutomationElement != null && propertyList.Any())
            {
                if (propertyList.Count > 1)
                {
                    List<string> values = new List<string>();
                    List<string> properties = new List<string>();

                    if (propertyList.Select(x => x.Value).OfType<PropertyCondition>().Count() != propertyList.Count())
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
                    GenerateXPathAndCache(string.Join("`", values.ToArray()), string.Join("`", properties.ToArray()), control);
                }
                else
                {
                    var firstProp = propertyList.First();
                    string value = firstProp.Value.Value.ToString();
                    if (firstProp.Ignore)
                    {
                        value = control.GetType().GetProperty(firstProp.Value.Property.Name).GetValue(control, null).ToString();
                        GenerateXPathAndCache(value, firstProp.Value.Property.Name, control);
                    }
                    else
                    {
                        GenerateXPathAndCache(value, firstProp.Value.Property.Name, control);
                    }
                }
            }
        }

        private void GenerateXPathAndCache(string identifier, string property, SHAutomationElement element)
        {

            if (!_xPathValues.Any())
            {
                _xPathValues.Add(SHAutomation.Core.Debug.GetXPathToElement(element));
                _hasXPathValue = false;
            }
            else
            {
                _hasXPathValue = true;
            }

            if (_xPathValues.Any() && !_hasXPathValue)
            {
                foreach (string str in _xPathValues)
                {
                    var items = str.Split('/').ToList();
                    // "" and Window
                    var lastItem = items.Last();
                    items.RemoveRange(0, 2);

                    lastItem = lastItem.Split('[')[0];

                    if (property.Contains("`"))
                    {
                        var splitAnds = identifier.Split(new string[] { "`" }, StringSplitOptions.None);
                        var splitProperties = property.Split(new string[] { "`" }, StringSplitOptions.None);
                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append("[");
                        for (int i = 0; i < splitAnds.Count() - 1; i++)
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
                        items.RemoveAt(items.Count - 1);
                    }
                    items.Add(lastItem);
                    string newStr = string.Join("/", items);

                    //If cache already contains the value then remove and replace with up-to-date version
                    if (XPathList.Any(x => x.identifier == identifier && x.xpath == ("/" + newStr)))
                    {
                        var removeItem = XPathList.FirstOrDefault(x => x.xpath == ("/" + newStr));
                        XPathList.Remove(removeItem);
                    }

                    XPathList.Add((identifier: identifier, property: property, xpath: "/" + newStr));
                }
            }
        }

        private bool GetXPathValueFromCache(string identifier, string property, out List<string> xPathValues)
        {
            xPathValues = new List<string>();

            if (XPathList.Any(x => x.identifier.ToUpper() == identifier.ToUpper() && x.property == property))
            {
                xPathValues = XPathList.Where(x => x.identifier.ToUpper() == identifier.ToUpper()).Select(x => x.xpath).ToList();

                return true;
            }
            else
                return false;
        }

        public SHAutomationElement FindFirstByXPath(string xpath, int? spinWaitTimeout = 1)
        {
            if (xpath.Contains("&quot;"))
            {
                xpath = xpath.Replace('\'', '"');
                xpath = xpath.Replace("&quot;", "'");
                xpath = xpath.Replace(@"\", "");
            }

            ISHAutomationElement element = null;

            bool getElement()
            {
                element = base.FindFirstByXPath(xpath);
                return element != null;
            }

            if (spinWaitTimeout != null)
                SHSpinWait.SpinUntil(() => getElement(), spinWaitTimeout.Value);
            else
            {
                getElement();
            }

            if (element != null)
            {
                SHSpinWait.SpinUntil(() => element?.FrameworkAutomationElement != null, 5000);
            }
            else if (element == null)
                _xPathValues = new List<string>();

            return element?.FrameworkAutomationElement != null ? (SHAutomationElement)element : null;
        }
        private SHAutomationElement FindFirstByXPath(List<string> xpath, int? spinWaitTimeout = 10000)
        {
            SHAutomationElement validXpath = null;
            if (spinWaitTimeout.HasValue)
            {
                SHSpinWait.SpinUntil(() => FoundXpathInList(xpath, out validXpath), spinWaitTimeout.Value);
            }
            else
            {
                FoundXpathInList(xpath, out validXpath);
            }
            if (validXpath == null)
            {
                _xPathValues = new List<string>();
                var output = string.Format("Failed to find control by xpath {0}", string.Join(",", xpath));
            }


            return validXpath?.FrameworkAutomationElement != null ? validXpath : null;
        }

        private bool FoundXpathInList(List<string> xpath, out SHAutomationElement xPathObject)
        {
            xPathObject = null;
            bool found = false;
            foreach (var x in xpath)
            {
                var tempXPathObject = FindFirstByXPath(x, null);
                if (tempXPathObject?.FrameworkAutomationElement != null)
                {
                    found = true;
                    xPathObject = tempXPathObject;
                    break;
                }
            }

            return found;


        }
    }
}
