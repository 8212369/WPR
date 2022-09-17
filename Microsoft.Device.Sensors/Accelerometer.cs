using System;

namespace Microsoft.Devices.Sensors
{
    public class Accelerometer : SensorBase<AccelerometerReading>
    {
        private bool _Started = false;

        public Accelerometer()
        {
            State = SensorState.Ready;
        }

#if __MOBILE__
        private void OnImplReadingChanged(object ?sender, Xamarin.Essentials.AccelerometerChangedEventArgs args)
        {
            // We always rotate it to the right... Which seems to be the opposite to Windows Phone default reported axis direction
            ReadingChanged?.Invoke(this, new AccelerometerReadingEventArgs(-args.Reading.Acceleration.X,
                -args.Reading.Acceleration.Y, -args.Reading.Acceleration.Z));

            OnCurrentValueChanged(new SensorReadingEventArgs<AccelerometerReading>()
            {
                SensorReading = new AccelerometerReading()
                {
                    Acceleration = new Microsoft.Xna.Framework.Vector3(-args.Reading.Acceleration.X,
                        -args.Reading.Acceleration.Y, -args.Reading.Acceleration.Z),
                    Timestamp = DateTimeOffset.Now
                }
            });
        }
#endif

        ~Accelerometer()
        {
            Stop();
        }

        public event EventHandler<AccelerometerReadingEventArgs>? ReadingChanged;

        public SensorState State { get; private set; }

        public void Start()
        {
            if (_Started)
            {
                return;
            }

            _Started = true;

#if __MOBILE__
            Xamarin.Essentials.Accelerometer.ReadingChanged += OnImplReadingChanged;

            if (!Xamarin.Essentials.Accelerometer.IsMonitoring)
            {
                Xamarin.Essentials.Accelerometer.Start(Xamarin.Essentials.SensorSpeed.Game);
            }
#endif
        }

        public void Stop()
        {
            if (!_Started)
            {
                return;
            }

#if __MOBILE__
            Xamarin.Essentials.Accelerometer.ReadingChanged -= OnImplReadingChanged;
#endif

            _Started = false;
        }
    }
}
