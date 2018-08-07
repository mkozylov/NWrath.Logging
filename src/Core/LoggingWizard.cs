using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class LoggingWizard
    {
        public static ILoggingWizardCharms Spell { get; set; } = new LoggingWizardCharms();
    }
}