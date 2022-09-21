namespace System.Device.Location
{
    public class GeoPositionChangedEventArgs<T> : EventArgs
    {
        public GeoPositionChangedEventArgs(GeoPosition<T> position)
        {
            Position = position;
        }

        public GeoPosition<T> Position { get; private set; }
    }
}
