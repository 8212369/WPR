namespace System.Device.Location
{
    public class GeoPositionStatusChangedEventArgs : EventArgs
    {
        public GeoPositionStatusChangedEventArgs(GeoPositionStatus status)
        {
            Status = status;
        }

        public GeoPositionStatus Status { get; private set; }
    }
}
