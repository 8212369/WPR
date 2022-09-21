using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class SignedInEventArgs : EventArgs
    {

        private SignedInGamer gamer;

        public SignedInEventArgs(SignedInGamer gamer)
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
