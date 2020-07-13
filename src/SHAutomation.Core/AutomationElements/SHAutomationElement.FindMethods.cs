using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.XPath;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {


        /// <summary>
        /// Finds the first element which is in the given treescope with the given condition.
        /// </summary>
        public SHAutomationElement FindFirstBase(TreeScope treeScope, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindFirst(treeScope, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Find all matching elements in the specified order.
        /// </summary>
        public SHAutomationElement[] FindAllWithOptionsBase(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions, SHAutomationElement root)
        {
            try
            {
                return FrameworkAutomationElement.FindAllWithOptions(treeScope, condition, traversalOptions, root);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the first matching element in the specified order.
        /// </summary>
        public SHAutomationElement FindFirstWithOptionsBase(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions, SHAutomationElement root)
        {
            try
            {
                return FrameworkAutomationElement.FindFirstWithOptions(treeScope, condition, traversalOptions, root);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        public SHAutomationElement FindFirstNested(params ConditionBase[] nestedConditions)
        {
            try
            {
                var currentElement = this;
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        public SHAutomationElement[] FindAllNested(params ConditionBase[] nestedConditions)
        {
            try
            {
                var currentElement = this;
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

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
                return (ISHAutomationElement)nodeItem?.UnderlyingObject;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
            catch (XPathException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all items which match the given xpath.
        /// </summary>
        private SHAutomationElement[] FindAllByXPathBase(string xPath)
        {
            try
            {
                var xPathNavigator = new SHAutomationElementXPathNavigator(this);
                var itemNodeIterator = xPathNavigator.Select(xPath);
                var itemList = new List<SHAutomationElement>();
                while (itemNodeIterator.MoveNext())
                {
                    var automationItem = (SHAutomationElement)itemNodeIterator.Current.UnderlyingObject;
                    itemList.Add(automationItem);
                }
                return itemList.ToArray();
            }
            catch (Exception ex) when (ex is System.Runtime.InteropServices.COMException || ex is NullReferenceException)
            {
                return Array.Empty<SHAutomationElement>();
            }
        }

        /// <summary>
        /// Finds the element which is in the given treescope with the given condition and the given index.
        /// </summary>
        public SHAutomationElement FindAt(TreeScope treeScope, int index, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindIndexed(treeScope, index, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child.
        /// </summary>
        /// <returns>The found element or null if no element was found.</returns>
        public SHAutomationElement FindFirstChildBase()
        {
            try
            {
                return FindFirstBase(TreeScope.Children, TrueCondition.Default);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child with the given automation id.
        /// </summary>
        /// <param name="automationId">The automation id.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public SHAutomationElement FindFirstChildBase(string automationId)
        {
            try
            {
                return FindFirstBase(TreeScope.Children, ConditionFactory.ByAutomationId(automationId));
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first child with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public SHAutomationElement FindFirstChildBase(ConditionBase condition)
        {
            try
            {
                return FindFirstBase(TreeScope.Children, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all children.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllChildrenBase()
        {
            try
            {
                return FindAllBase(TreeScope.Children, TrueCondition.Default);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllChildrenBase(ConditionBase condition)
        {
            try
            {
                return FindAllBase(TreeScope.Children, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds all children with the condition.
        /// </summary>
        /// <param name="conditionFunc">The condition mehtod.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllChildrenBase(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindAllChildrenBase(condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

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
            catch (System.Runtime.InteropServices.COMException)
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first descendant with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public SHAutomationElement FindFirstDescendantBase(ConditionBase condition)
        {
            try
            {
                return FindFirstBase(TreeScope.Descendants, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all descendants.
        /// </summary>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllDescendantsBase()
        {
            try
            {
                return FindAllBase(TreeScope.Descendants, TrueCondition.Default);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();
            }
        }

        /// <summary>
        /// Finds all descendants with the condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllDescendantsBase(ConditionBase condition)
        {
            try
            {
                return FindAllBase(TreeScope.Descendants, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
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
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }

        /// <summary>
        /// Finds the first element by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found element or null if no element was found.</returns>
        public SHAutomationElement FindFirstNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            try
            {
                var conditions = conditionFunc(ConditionFactory);
                return FindFirstNested(conditions.ToArray());
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all elements by iterating thru all conditions.
        /// </summary>
        /// <param name="conditionFunc">The condition method.</param>
        /// <returns>The found elements or an empty list if no elements were found.</returns>
        public SHAutomationElement[] FindAllNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc)
        {
            try
            {
                var conditions = conditionFunc(ConditionFactory);
                return FindAllNested(conditions.ToArray());
            }
            catch (System.Runtime.InteropServices.COMException)
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
        public SHAutomationElement FindChildAt(int index, ConditionBase condition = null)
        {
            try
            {
                return FindAt(TreeScope.Children, index, condition ?? TrueCondition.Default);
            }
            catch (System.Runtime.InteropServices.COMException)
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
        public SHAutomationElement FindChildAt(int index, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            try
            {
                var condition = conditionFunc(ConditionFactory);
                return FindChildAt(index, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }
        public ISHAutomationElement FindFirstChild()
        {
            return FindFirstChild(20000);
        }
        public ISHAutomationElement FindFirstChild(int timeout = 20000, bool waitUntilExists = true)
        {
          SHAutomationElement element = null;
            bool getElement(bool shouldExist)
            {
                if (element == null)
                {
                    element = FindFirstChildBase();
                }
                return shouldExist ? element?.FrameworkAutomationElement != null : element == null;
            }
            getElement(waitUntilExists);
            if (element == null && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(true), timeout);
            }
            else if (element != null && !waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(false), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
        public  ISHAutomationElement FindFirstChild(ConditionBase condition)
        {
            return FindFirstChild(condition, 20000);
        }
        public ISHAutomationElement FindFirstChild(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true)
        {
          SHAutomationElement element = null;
            bool getElement(bool shouldExist)
            {
                if (element == null)
                {
                    element = FindFirstChildBase(condition);
                }
                return shouldExist ? element?.FrameworkAutomationElement != null : element == null;
            }
            getElement(waitUntilExists);
            if (element == null && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(true), timeout);
            }
            else if (element != null && !waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(false), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
        public ISHAutomationElement FindFirstChild(string automationId)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition);
        }
        public ISHAutomationElement FindFirstChild(string automationId, int timeout = 20000, bool waitUntilExists = true)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition, timeout, waitUntilExists);
        }
        public ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstChild(condition,timeout,waitUntilExists);
        }

    }
}
