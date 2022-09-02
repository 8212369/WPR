using System;

namespace Microsoft.Devices
{
    public class VibrateController
    {
        private static VibrateController? _Default;

        public static VibrateController Default
        {
            get
            {
                if (_Default == null)
                {
                    _Default = new VibrateController();
                }

                return _Default;
            }
        }

        public void Start(TimeSpan duration)
        {
        }

        public void Stop()
        {

        }
    }
}
