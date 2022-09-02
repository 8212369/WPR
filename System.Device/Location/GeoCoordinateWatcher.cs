namespace System.Device.Location
{
    public class GeoCoordinateWatcher
    {
        private GeoPositionAccuracy _Accuracy;
        private double _Threshold;

        public event EventHandler<GeoLocationChangedEventArgs> LocationChanged;
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        public GeoCoordinateWatcher(GeoPositionAccuracy accuracy)
        {
            _Accuracy = accuracy;
        }

        public double MovementThreshold
        {
            get => _Threshold;
            set {
                if (value < 0.0 || Double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException("Threshold value is set to negative!");
                }

                _Threshold = value;
            }
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}