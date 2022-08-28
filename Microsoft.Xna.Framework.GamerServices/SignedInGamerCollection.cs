using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class SignedInGamerCollection : GamerCollection<SignedInGamer>
    {
        internal SignedInGamerCollection()
        {
        }

        public SignedInGamer this[PlayerIndex index]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    SignedInGamer gamer = base[i];
                    if (gamer.PlayerIndex == index)
                    {
                        return gamer;
                    }
                }
                return null;
            }
        }
    }
}
