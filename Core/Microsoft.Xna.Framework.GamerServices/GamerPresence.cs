using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GamerPresence
    {
        private GamerPresenceMode _GamerPresenceMode = GamerPresenceMode.None;
        private int _PresenceValue = 0;

        internal GamerPresence()
        {
        }

        public GamerPresenceMode PresenceMode
        {
            get
            {
                return _GamerPresenceMode;
            }
            set
            {
                _GamerPresenceMode = value;
            }
        }

        public int PresenceValue
        {
            get
            {
                return _PresenceValue;
            }
            set
            {
                _PresenceValue = value;
            }
        }
    }
}
