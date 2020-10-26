using SHAutomation.Core;
using SHAutomation.Core.AutomationElements.Infrastructure;
using SHAutomation.Core.Patterns;
using SHAutomation.UIA3.Patterns;

namespace SHAutomation.UIA3
{
    public class UIA3EventLibrary : IEventLibrary
    {
        public UIA3EventLibrary()
        {
            Element = new UIA3AutomationElementEventIds();
            Drag = new DragPatternEventIds();
            DropTarget = new DropTargetPatternEventIds();
            Invoke = new InvokePatternEventIds();
            SelectionItem = new SelectionItemPatternEventIds();
            Selection = new SelectionPatternEventIds();
            SynchronizedInput = new SynchronizedInputPatternEventIds();
            TextEdit = new TextEditPatternEventIdIds();
            Text = new TextPatternEventIds();
            Window = new WindowPatternEventIds();
        }

        public ISHAutomationElementEventIds Element { get; }
        public IDragPatternEventIds Drag { get; }
        public IDropTargetPatternEventIds DropTarget { get; }
        public IInvokePatternEventIds Invoke { get; }
        public ISelectionItemPatternEventIds SelectionItem { get; }
        public ISelectionPatternEventIds Selection { get; }
        public ISynchronizedInputPatternEventIds SynchronizedInput { get; }
        public ITextEditPatternEventIds TextEdit { get; }
        public ITextPatternEventIds Text { get; }
        public IWindowPatternEventIds Window { get; }
    }
}
