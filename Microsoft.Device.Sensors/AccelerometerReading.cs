using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Devices.Sensors
{
    public struct AccelerometerReading : ISensorReading
    {
        public DateTimeOffset Timestamp { get; internal set; }

        public Vector3 Acceleration { get; internal set; }
    }
}