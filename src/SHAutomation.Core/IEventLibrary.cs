using StreetsHeaver.SHAutomation.Core.AutomationElements.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Patterns;

namespace StreetsHeaver.SHAutomation.Core
{
    public interface IEventLibrary
    {
        ISHAutomationElementEventIds Element { get; }
        IDragPatternEventIds Drag { get; }
        IDropTargetPatternEventIds DropTarget { get; }
        IInvokePatternEventIds Invoke { get; }
        ISelectionItemPatternEventIds SelectionItem { get; }
        ISelectionPatternEventIds Selection { get; }
        ISynchronizedInputPatternEventIds SynchronizedInput { get; }
        ITextEditPatternEventIds TextEdit { get; }
        ITextPatternEventIds Text { get; }
        IWindowPatternEventIds Window { get; }
    }
}
