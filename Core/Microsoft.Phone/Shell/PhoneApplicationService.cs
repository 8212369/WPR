using System;
using System.Collections.Generic;

namespace Microsoft.Phone.Shell
{
    public class PhoneApplicationService
    {
        private bool _AppActivated = false;

        static PhoneApplicationService()
        {
            Current = new PhoneApplicationService();
        }

        public PhoneApplicationService()
        {
            UserIdleDetectionMode = IdleDetectionMode.Disabled;
            ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;

            State = new Dictionary<string, object>();
        }

        public void HandleApplicationStart(bool anew)
        {
            _Activated?.Invoke(this, new ActivatedEventArgs(!anew));
            _AppActivated = true;
        }

        public void HandleApplicationExit()
        {
            Deactivated?.Invoke(this, new DeactivatedEventArgs());
            Closing?.Invoke(this, new ClosingEventArgs());

            // Recycle
            Current = new PhoneApplicationService();
        }

        private event EventHandler<ActivatedEventArgs>? _Activated;

        public event EventHandler<ActivatedEventArgs>? Activated
        {
            add
            {
                if (_AppActivated)
                {
                    value?.Invoke(this, new ActivatedEventArgs(false));
                } else
                {
                    _Activated += value;
                }
            }

            remove
            {
                _Activated -= value;
            }
        }
        public event EventHandler<DeactivatedEventArgs>? Deactivated;
        public event EventHandler<LaunchingEventArgs>? Launching;
        public event EventHandler<ClosingEventArgs>? Closing;

        public StartupMode StartupMode { get => StartupMode.Launch; }

        public static PhoneApplicationService? Current { get; private set; }

        public IDictionary<string, object> State { get; private set; }

        public IdleDetectionMode UserIdleDetectionMode { get; set; }
        public IdleDetectionMode ApplicationIdleDetectionMode { get; set; }
    }
}
