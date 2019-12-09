using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWrath.Logging
{
    public static class EmptyLoggerWizardExtensions
    {
        #region Empty

        //1
        public static EmptyLogger EmptyLogger(
            this LoggingWizardCharms charms
            )
        {
            return new EmptyLogger();
        }

        #endregion Empty

    }
}
