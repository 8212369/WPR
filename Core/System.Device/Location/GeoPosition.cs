
using System.Collections.Generic;

namespace System.Device.Location
{
    public class GeoPosition<T>
    {
        private DateTimeOffset _Timestamp = DateTimeOffset.MinValue;
        private T _Position;

        public GeoPosition() :
            this(DateTimeOffset.MinValue, default(T)!)
        {
        }

        public GeoPosition(DateTimeOffset timestamp, T position)
        {
            _Timestamp = timestamp;
            _Position = position;
        }

        public DateTimeOffset Timestamp
        {
            get => _Timestamp;
            set => _Timestamp = value;
        }

        public T Location
        {
            get => _Position;
            set => _Position = value;
        }
    }
}
