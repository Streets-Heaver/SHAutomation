using SHAutomation.Core.AutomationElements;

namespace SHAutomation.Core
{
    /// <summary>
    /// Interface for a class that implements a tree walker.
    /// </summary>
    public interface ITreeWalker
    {
        /// <summary>
        /// Gets the parent of the given element.
        /// </summary>
        /// <param name="element">The element to get the parent for.</param>
        /// <returns>The parent or null if none is found.</returns>
      SHAutomationElement GetParent(SHAutomationElement element);

        /// <summary>
        /// Gets the first child of the given element.
        /// </summary>
        /// <param name="element">The element to get the first child for.</param>
        /// <returns>The first child or null if none is found.</returns>
      SHAutomationElement GetFirstChild(SHAutomationElement element);

        /// <summary>
        /// Gets the last child of the given element.
        /// </summary>
        /// <param name="element">The element to get the last child for.</param>
        /// <returns>The last child or null if none is found.</returns>
      SHAutomationElement GetLastChild(SHAutomationElement element);

        /// <summary>
        /// Gets the next sibling of the given element.
        /// </summary>
        /// <param name="element">The element to get the next sibling for.</param>
        /// <returns>The next sibling or null if none is found.</returns>
      SHAutomationElement GetNextSibling(SHAutomationElement element);

        /// <summary>
        /// Gets the previous sibling of the given element.
        /// </summary>
        /// <param name="element">The element to get the previous sibling for.</param>
        /// <returns>The previous sibling or null if none is found.</returns>
      SHAutomationElement GetPreviousSibling(SHAutomationElement element);
    }
}
