using System;

namespace Microsoft.Phone.Shell
{
    public class DeactivatedEventArgs : EventArgs
    {
        private DeactivationReason _reason;

        public DeactivatedEventArgs() => this._reason = DeactivationReason.UserAction;

        public DeactivatedEventArgs(DeactivationReason reason) => this._reason = reason;

        public DeactivationReason Reason => this._reason;
    }
}
