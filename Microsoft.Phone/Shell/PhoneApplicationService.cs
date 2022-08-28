using System;

namespace Microsoft.Phone.Shell
{
    public class PhoneApplicationService
    {
        static PhoneApplicationService()
        {
            Current = new PhoneApplicationService();
        }

        public event EventHandler<DeactivatedEventArgs>? Deactivated;

        public StartupMode StartupMode { get => StartupMode.Launch; }

        public static PhoneApplicationService? Current { get; private set; }
    }
}
