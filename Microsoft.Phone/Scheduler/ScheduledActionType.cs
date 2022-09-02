using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Scheduler
{
    public enum ScheduledActionType
    {
        Alarm,
        Reminder,
        PeriodicTask,
        OnIdleTask,
        VoipKeepAliveTask,
        VoipHttpIncomingCallTask,
        IncomingMobileNetworkMessageTask,
    }
}
