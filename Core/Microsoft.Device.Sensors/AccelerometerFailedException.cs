using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Devices.Sensors
{
    [Serializable]
    public class AccelerometerFailedException : Exception
    {
        public AccelerometerFailedException()
        {
        }

        public AccelerometerFailedException(string message)
            : base(message)
        {
        }

        public AccelerometerFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
