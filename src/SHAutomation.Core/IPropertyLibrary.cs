﻿using SHAutomation.Core.AutomationElements.Infrastructure;
using SHAutomation.Core.Patterns;

namespace SHAutomation.Core
{
    public interface IPropertyLibrary
    {
        ISHAutomationElementPatternAvailabilityPropertyIds PatternAvailability { get; }
        ISHAutomationElementPropertyIds Element { get; }
        IAnnotationPatternPropertyIds Annotation { get; }
        IDockPatternPropertyIds Dock { get; }
        IDragPatternPropertyIds Drag { get; }
        IDropTargetPatternPropertyIds DropTarget { get; }
        IExpandCollapsePatternPropertyIds ExpandCollapse { get; }
        IGridItemPatternPropertyIds GridItem { get; }
        IGridPatternPropertyIds Grid { get; }
        ILegacyIAccessiblePatternPropertyIds LegacyIAccessible { get; }
        IMultipleViewPatternPropertyIds MultipleView { get; }
        IRangeValuePatternPropertyIds RangeValue { get; }
        IScrollPatternPropertyIds Scroll { get; }
        ISelection2PatternPropertyIds Selection2 { get; }
        ISelectionItemPatternPropertyIds SelectionItem { get; }
        ISelectionPatternPropertyIds Selection { get; }
        ISpreadsheetItemPatternPropertyIds SpreadsheetItem { get; }
        IStylesPatternPropertyIds Styles { get; }
        ITableItemPatternPropertyIds TableItem { get; }
        ITablePatternPropertyIds Table { get; }
        ITogglePatternPropertyIds Toggle { get; }
        ITransform2PatternPropertyIds Transform2 { get; }
        ITransformPatternPropertyIds Transform { get; }
        IValuePatternPropertyIds Value { get; }
        IWindowPatternPropertyIds Window { get; }
    }
}
