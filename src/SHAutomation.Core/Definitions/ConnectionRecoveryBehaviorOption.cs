using System;
using System.Collections.Generic;
using System.Text;

namespace SHAutomation.Core.Definitions
{
    /// <summary>
    /// Contains possible values for the ConnectionRecoveryBehavior property,
    /// which indicates whether an accessible technology client adjusts provider
    /// request timeouts when the provider is non-responsive.
    /// </summary>
    public enum ConnectionRecoveryBehaviorOption
    {
        /// <summary>
        /// Connection recovery is disabled.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Connection recovery is enabled.
        /// </summary>
        Enabled = 1
    }
}
