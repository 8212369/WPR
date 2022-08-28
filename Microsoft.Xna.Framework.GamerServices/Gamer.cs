using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public abstract class Gamer
    {
        internal Gamer()
        {
        }

        public IAsyncResult BeginGetProfile(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public GamerProfile EndGetProfile(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public GamerProfile GetProfile()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public string Gamertag
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsDisposed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static SignedInGamerCollection SignedInGamers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object Tag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
