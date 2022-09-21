using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class FriendCollection : GamerCollection<FriendGamer>, IDisposable
    {
        internal FriendCollection()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public bool IsDisposed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
