using SHAutomation.Core.Caching;
using SHAutomation.Core.Conditions;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace SHAutomation.Core.AutomationElements
{
    public interface IWindow
    {
        ICacheService CacheService { get; set; }
        Menu ContextMenu { get; }
        IFileSystem FileSystem { get; set; }
        bool IsModal { get; }
        Window[] ModalWindows { get; }
        Window Popup { get; }
        string Title { get; }
        TitleBar TitleBar { get; }
        List<(string identifier, string property, string xpath)> XPathList { get; }

        void Close();
        bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false);
        bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, TimeSpan offscreenTimeout, bool checkOnScreen = true, ISHAutomationElement parent = null, bool xpathOnly = false);
        ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, ISHAutomationElement parent = null);
        ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, ISHAutomationElement parent = null);
        ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, TimeSpan offscreenTimeout, ISHAutomationElement parent = null);
        ISHAutomationElement Find(string automationID, ISHAutomationElement parent = null);
        ISHAutomationElement Find(string automationID, TimeSpan timeout, ISHAutomationElement parent = null);
        ISHAutomationElement Find(string automationID, TimeSpan timeout, TimeSpan offscreenTimeout, ISHAutomationElement parent = null);
        SHAutomationElement FindFirstByXPath(string xpath, TimeSpan spinWaitTimeout);
        Menu GetContextMenuByFrameworkType(FrameworkType frameworkType);
        List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(ConditionBase condition);
        void GetXPathCache(string testName);
        SHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc);
        SHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout);
        void Move(int x, int y);
        void SaveXPathCache(string testName);
        void SaveXPathFromControl(SHAutomationElement control, Func<ConditionFactory, ConditionBase> conditionFunc, bool regenerateXPath);
        void SaveXPathFromControl(SHAutomationElement control, List<(PropertyCondition Value, bool Ignore)> propertyList, bool regenerateXPath);
        void SetTransparency(byte alpha);
    }
}