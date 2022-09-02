using System;
using System.Collections.Generic;

namespace Microsoft.Phone.Shell
{
    public class PhoneApplicationService
    {
        static PhoneApplicationService()
        {
            Current = new PhoneApplicationService();
        }

        public PhoneApplicationService()
        {
            UserIdleDetectionMode = IdleDetectionMode.Disabled;
            ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        public void HandleOneFrameRunDone(bool anew)
        {
            Activated?.Invoke(this, new ActivatedEventArgs(!anew));
        }

        public void HandleApplicationExit()
        {
            Deactivated?.Invoke(this, new DeactivatedEventArgs());
            Closing?.Invoke(this, new ClosingEventArgs());

            // Recycle
            Current = new PhoneApplicationService();
        }

        public event EventHandler<ActivatedEventArgs>? Activated;
        public event EventHandler<DeactivatedEventArgs>? Deactivated;
        public event EventHandler<LaunchingEventArgs>? Launching;
        public event EventHandler<ClosingEventArgs>? Closing;

        public StartupMode StartupMode { get => StartupMode.Launch; }

        public static PhoneApplicationService? Current { get; private set; }

        public IDictionary<string, object>? State { get; private set; }

        public IdleDetectionMode UserIdleDetectionMode { get; set; }
        public IdleDetectionMode ApplicationIdleDetectionMode { get; set; }
    }
}
