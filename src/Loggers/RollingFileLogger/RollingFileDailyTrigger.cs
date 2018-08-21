using NWrath.Synergy.Common;
using NWrath.Synergy.Pipeline;
using System;

namespace NWrath.Logging
{
    public class RollingFileDailyTrigger
        : PipeBase<RollingFileContext>
    {
        private IRollingFileAction[] _actionTriggers;
        private DateTime _nextCheck;

        public RollingFileDailyTrigger(
            IRollingFileAction actionTrigger,
            bool instantNext = true
            )
            : this(new[] { actionTrigger }, instantNext)
        {
        }

        public RollingFileDailyTrigger(
            IRollingFileAction[] actionTriggers,
            bool instantNext = true
            )
        {
            _actionTriggers = actionTriggers;
            _nextCheck = instantNext ? Clock.Now
                                     : Clock.Today.AddDays(1);
        }

        public override void Perform(RollingFileContext context)
        {
            if (Predicate(context))
            {
                foreach (var action in _actionTriggers)
                {
                    action.Execute(context);
                }
            }

            PerformNext(context);
        }

        public bool Predicate(RollingFileContext ctx)
        {
            var now = Clock.Now;

            var timeIsUp = now >= _nextCheck;

            if (timeIsUp)
            {
                _nextCheck = Clock.Today.AddDays(1);
            }

            return timeIsUp;
        }
    }
}