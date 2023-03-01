using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {


        /// <summary>
        /// Finds the first element which is in the given treescope with the given condition.
        /// </summary>
        public ISHAutomationElement FindFirstBase(TreeScope treeScope, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindFirst(treeScope, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Find all matching elements in the specified order.
        /// </summary>
        public ISHAutomationElement[] FindAllWithOptionsBase(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions, ISHAutomationElement root)
        {
            try
            {
                return FrameworkAutomationElement.FindAllWithOptions(treeScope, condition, traversalOptions, root);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the first matching element in the specified order.
        /// </summary>
        public ISHAutomationElement FindFirstWithOptionsBase(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions, ISHAutomationElement root)
        {
            try
            {
                return FrameworkAutomationElement.FindFirstWithOptions(treeScope, condition, traversalOptions, root);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        public ISHAutomationElement FindFirstNested(params ConditionBase[] nestedConditions)
        {
            try
            {
                ISHAutomationElement currentElement = this;
                foreach (var condition in nestedConditions)
                {
                    currentElement = currentElement.FindFirstChildBase(condition);
                    if (currentElement == null)
                    {
                        return null;
                    }
                }
                return currentElement;
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        public ISHAutomationElement[] FindAllNested(params ConditionBase[] nestedConditions)
        {
            try
            {
                ISHAutomationElement currentElement = this;
                for (var i = 0; i < nestedConditions.Length - 1; i++)
                {
                    var condition = nestedConditions[i];
                    currentElement = currentElement.FindFirstChildBase(condition);
                    if (currentElement == null)
                    {
                        return null;
                    }
                }
                return currentElement.FindAllChildrenBase(nestedConditions.Last());
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<ISHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds for the first item which matches the given xpath.
        /// </summary>
        public ISHAutomationElement FindFirstByXPath(string xPath)
        {
            try
            {
                var xPathNavigator = new SHAutomationElementXPathNavigator(this);
                var nodeItem = xPathNavigator.SelectSingleNode(xPath);
                if (nodeItem?.UnderlyingObject != null)
                {
                    var element = (ISHAutomationElement)nodeItem?.UnderlyingObject;
                    element.FoundByXPath = true;
                    return element;
                }
                return null;
            }
            catch (Exception ex) when (ex is COMException || ex is NullReferenceException || ex is XPathException || ex is InvalidOperationException)
            {
                return null;
            }
        }
        public ISHAutomationElement FindFirstByXPath(string xPath, TimeSpan timeout)
        {
            ISHAutomationElement element = null;
            Func<ISHAutomationElement> function = () => FindFirstByXPath(xPath);

            Wait.UntilTrue(() => GetElement(function, true, ref element), timeout);
            if (element != null)
            {
                element.FoundByXPath = true;
            }
            return element;
        }
        /// <summary>
        /// Finds all items which match the given xpath.
        /// </summary>
        private ISHAutomationElement[] FindAllByXPathBase(string xPath)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var xPathNavigator = new SHAutomationElementXPathNavigator(this);
                var itemNodeIterator = xPathNavigator.Select(xPath);
                var itemList = new List<SHAutomationElement>();
                //keep moving to next node, if xpath search is longer than 2 minutes end search
                while (itemNodeIterator.MoveNext() && DateTime.Now.Subtract(startTime).TotalMinutes < 2)
                {
                    var automationItem = (SHAutomationElement)itemNodeIterator.Current.UnderlyingObject;
                    if (automationItem?.FrameworkAutomationElement != null)
                    {
                        automationItem.FoundByXPath = true;
                        itemList.Add(automationItem);
                    }
                }
                return itemList.ToArray();
            }
            catch (Exception ex) when (ex is COMException || ex is NullReferenceException || ex is XPathException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();
            }
        }

        /// <summary>
        /// Finds the element which is in the given treescope with the given condition and the given index.
        /// </summary>
        public ISHAutomationElement FindAt(TreeScope treeScope, int index, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindIndexed(treeScope, index, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child.
        /// </summary>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstChildBase()
        {
            try
            {
                return FindFirstBase(TreeScope.Children, TrueCondition.Default);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child with the given automation id.
        /// </summary>
        /// <param name="automationId">The automation id.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstChildBase(string automationId)
        {
            try
            {
                return FindFirstBase(TreeScope.Children, ConditionFactory.ByAutomationId(automationId));
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstChildBase(ConditionBase condition)
        {
            try
            {
                return FindFirstBase(TreeScope.Children, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindFirstChildBase(condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all children.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllChildrenBase()
        {
            try
            {
                return FindAllBase(TreeScope.Children, TrueCondition.Default);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllChildrenBase(ConditionBase condition)
        {
            try
            {
                return FindAllBase(TreeScope.Children, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllChildrenBase(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindAllChildrenBase(condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<ISHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the first descendant.
        /// </summary>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstDescendantBase()
        {
            try
            {
                return FindFirstBase(TreeScope.Descendants, TrueCondition.Default);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first descendant with the given automation id.
        /// </summary>
        /// <param name="automationId">The automation id.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstDescendantBase(string automationId)
        {
            try
            {
                return FindFirstBase(TreeScope.Descendants, ConditionFactory.ByAutomationId(automationId));
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first descendant with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstDescendantBase(ConditionBase condition)
        {
            try
            {
                return FindFirstBase(TreeScope.Descendants, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first descendant with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstDescendantBase(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindFirstDescendantBase(condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all descendants.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllDescendantsBase()
        {
            try
            {
                return FindAllBase(TreeScope.Descendants, TrueCondition.Default);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();
            }
        }

        /// <summary>
        /// Finds all descendants with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllDescendantsBase(ConditionBase condition)
        {
            try
            {
                return FindAllBase(TreeScope.Descendants, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();
            }
        }

        /// <summary>
        /// Finds all descendants with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllDescendantsBase(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindAllDescendantsBase(condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindFirstNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            try
            {
                var conditions = conditionFunc(ConditionFactory);
                return FindFirstNested(conditions.ToArray());
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public ISHAutomationElement[] FindAllNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            try
            {
                var conditions = conditionFunc(ConditionFactory);
                return FindAllNested(conditions.ToArray());
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the child at the given position with the condition.
        /// </summary>
        /// <param name="index">The index of the child to find.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindChildAt(int index, ConditionBase condition = null)
        {
            try
            {
                return FindAt(TreeScope.Children, index, condition ?? TrueCondition.Default);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the child at the given position with the condition.
        /// </summary>
        /// <param name="index">The index of the child to find.</param>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public ISHAutomationElement FindChildAt(int index, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindChildAt(index, condition);
            }
            catch (Exception ex) when (ex is COMException || ex is InvalidOperationException)
            {
                return null;
            }
        }
        public ISHAutomationElement FindFirstChild(bool waitUntilExists = true)
        {
            return FindFirstChild(TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirstChild(TimeSpan timeout, bool waitUntilExists = true)
        {
            ISHAutomationElement element = null;
            Func<ISHAutomationElement> function = () => FindFirstChildBase();
            GetElement(() => FindFirstChildBase(), waitUntilExists, ref element);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, true, ref element), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, false, ref element), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
        public bool GetElement(Func<ISHAutomationElement> function, bool shouldExist, ref ISHAutomationElement element)
        {
            if (element == null)
            {
                element = function.Invoke();
            }
            return shouldExist ? element?.FrameworkAutomationElement != null : element == null;
        }
        public bool GetElements(Func<List<ISHAutomationElement>> function, bool shouldExist, ref List<ISHAutomationElement> elements)
        {
            if (!elements.Any())
            {
                elements = function.Invoke();
            }
            return shouldExist ? elements.Any() : !elements.Any();
        }
        public ISHAutomationElement FindFirstChild(ConditionBase condition, bool waitUntilExists = true)
        {
            return FindFirstChild(condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirstChild(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            ISHAutomationElement element = null;
            Func<ISHAutomationElement> function = () => FindFirstChildBase(condition);
            GetElement(function, waitUntilExists, ref element);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, true, ref element), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, false, ref element), timeout);

            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
        public ISHAutomationElement FindFirstChild(string automationId)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition);
        }

        public ISHAutomationElement FindFirstChild(string automationId, bool waitUntilExists = true)
        {
            return FindFirstChild(automationId, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement FindFirstChild(string automationId, TimeSpan timeout, bool waitUntilExists = true)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition, timeout, waitUntilExists);
        }

        public ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true)
        {
            return FindFirstChild(conditionFunc, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition, timeout, waitUntilExists);
        }

    }
}
