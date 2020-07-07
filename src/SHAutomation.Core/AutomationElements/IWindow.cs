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

        void Close();
        bool Exists(Func<ConditionFactory, ConditionBase> conditionFunc, bool checkOnScreen = true, int timeout = 500, int offscreenTimeout = 500, ISHAutomationElement parent = null, bool xpathOnly = false);
        ISHAutomationElement Find(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, int offscreenTimeout = 10000, ISHAutomationElement parent = null);
        ISHAutomationElement Find(string automationID, int timeout = 20000, int offscreenTimeout = 10000, ISHAutomationElement parent = null);
        SHAutomationElement FindFirstByXPath(string xpath, int? spinWaitTimeout = 1);
        Menu GetContextMenuByFrameworkType(FrameworkType frameworkType);
        List<(PropertyCondition Value, bool Ignore)> GetPropertyConditions(ConditionBase condition);
        void GetXPathCache(string testName);
        SHAutomationElement GetXPathElementFromCondition(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 10000);
        void Move(int x, int y);
        void SaveXPathCache(string testName);
        void SaveXPathFromControl(SHAutomationElement control, Func<ConditionFactory, ConditionBase> conditionFunc);
        void SaveXPathFromControl(SHAutomationElement control, List<(PropertyCondition Value, bool Ignore)> propertyList);
        void SetTransparency(byte alpha);
    }
}