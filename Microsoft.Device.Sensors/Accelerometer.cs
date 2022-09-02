using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Devices.Sensors
{
    public class Accelerometer : SensorBase<AccelerometerReading>
    {
        public Accelerometer()
        {
            State = SensorState.Ready;
        }

        public event EventHandler<AccelerometerReadingEventArgs>? ReadingChanged;

        public SensorState State { get; private set; }

        public void Start()
        {
        }
    }
}
