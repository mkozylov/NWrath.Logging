using System;
using System.Collections.Generic;
using System.Text;
using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public class LoggingWizardCharms
        : ILoggingWizardCharms
    {
        public virtual Set Library { get; set; } = Set.Empty;
    }
}