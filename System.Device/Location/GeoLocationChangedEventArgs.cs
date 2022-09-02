namespace System.Device.Location
{
    public class GeoLocationChangedEventArgs : EventArgs
    {
        public GeoLocationChangedEventArgs(GeoLocation location)
        {
            Location = location;
        }

        public GeoLocation Location { get; private set; }
    }
}
