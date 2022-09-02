using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Scheduler
{
    public class ScheduledTask : ScheduledAction
    {
        public string? Description { get; set; }
        public AgentExitReason LastExitReason { get; internal set; }
        public DateTime LastScheduledTime { get; internal set; }

        public ScheduledTask(string name, ScheduledActionType actionType)
        {
            this.Name = name;
            this.Type = actionType;
        }
    }
}
