using System;

namespace Microsoft.Phone.Scheduler
{
    public class PeriodicTask : ScheduledTask
    {
        public PeriodicTask(string name)
            : base(name, ScheduledActionType.PeriodicTask)
        {

        }
    }
}
