using System.Linq;
using System.Text;
using SHAutomation.Core.WindowsAPI;

namespace SHAutomation.Core.Tools
{
    public static class AccessibilityTextResolver
    {
        public static string GetRoleText(AccessibilityRole role)
        {
            var sb = new StringBuilder(1024);
            _ = Oleacc.GetRoleText(role, sb, 1024);
            return sb.ToString();
        }

        public static string GetStateBitText(AccessibilityState state)
        {
            var sb = new StringBuilder(1024);
            _ = Oleacc.GetStateText(state, sb, 1024);
            return sb.ToString();
        }

        public static string GetStateText(AccessibilityState state)
        {
            var allStates = state.GetFlags();
            return string.Join(", ", allStates.Select(s => GetStateBitText((AccessibilityState)s)).ToArray());
        }
    }
}
