using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Scheduler
{
    public class ScheduledAction
    {
        public virtual DateTime BeginTime { get; set; }
        public virtual DateTime ExpirationTime { get; set; }
        public bool IsEnabled { get; }
        public bool IsScheduled { get; }

        protected ScheduledActionType Type { get; set; }

        internal string? Name { get; set; }
    }
}
