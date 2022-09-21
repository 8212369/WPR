using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Devices.Sensors
{
    public enum SensorState
    {
        NotSupported,
        Ready,
        Initializing,
        NoData,
        NoPermissions,
        Disabled,
    }
}
