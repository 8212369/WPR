using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class SignedOutEventArgs : EventArgs
    {
        private SignedInGamer gamer;

        public SignedOutEventArgs(SignedInGamer gamer)
        {
            if (gamer == null)
            {
                throw new ArgumentNullException("gamer");
            }
            this.gamer = gamer;
        }

        public SignedInGamer Gamer
        {
            get
            {
                return this.gamer;
            }
        }

    }
}
