using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class SignedInGamer : Gamer
    {

        public event EventHandler<EventArgs> AvatarChanged;
        public static event EventHandler<SignedInEventArgs> SignedIn;
        public static event EventHandler<SignedOutEventArgs> SignedOut;

        internal SignedInGamer()
        {
        }

        public FriendCollection GetFriends()
        {
            throw new NotImplementedException();
        }

        public bool IsFriend(Gamer gamer)
        {
            throw new NotImplementedException();
        }

        public AvatarDescription Avatar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GameDefaults GameDefaults
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGuest
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSignedInToLive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PartySize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PlayerIndex PlayerIndex
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GamerPresence Presence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GamerPrivileges Privileges
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
