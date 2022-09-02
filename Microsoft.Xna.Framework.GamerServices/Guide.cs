using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class Guide
    {
        public static bool _SimulateTrialMode = false;

        public static IAsyncResult BeginShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowMessageBox(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowMessageBox(PlayerIndex player, string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowStorageDeviceSelector(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowStorageDeviceSelector(PlayerIndex player, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowStorageDeviceSelector(int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static IAsyncResult BeginShowStorageDeviceSelector(PlayerIndex player, int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public static void DelayNotifications(TimeSpan delay)
        {
            throw new NotImplementedException();
        }

        public static string EndShowKeyboardInput(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public static int? EndShowMessageBox(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public static void ShowComposeMessage(PlayerIndex player, string text, IEnumerable<Gamer> recipients)
        {
            throw new NotImplementedException();
        }

        public static void ShowFriendRequest(PlayerIndex player, Gamer gamer)
        {
            throw new NotImplementedException();
        }

        public static void ShowFriends(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowGameInvite(PlayerIndex player, IEnumerable<Gamer> recipients)
        {
            throw new NotImplementedException();
        }

        public static void ShowGamerCard(PlayerIndex player, Gamer gamer)
        {
            throw new NotImplementedException();
        }

        public static void ShowMarketplace(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowMessages(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowParty(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowPartySessions(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowPlayerReview(PlayerIndex player, Gamer gamer)
        {
            throw new NotImplementedException();
        }

        public static void ShowPlayers(PlayerIndex player)
        {
            throw new NotImplementedException();
        }

        public static void ShowSignIn(int paneCount, bool onlineOnly)
        {
        }

        public static bool IsScreenSaverEnabled
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public static bool IsTrialMode
        {
            get
            {
                return false;
            }
        }

        public static bool IsVisible
        {
            get
            {
                return false;
            }
        }

        public static NotificationPosition NotificationPosition
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

        public static bool SimulateTrialMode
        {
            get => _SimulateTrialMode;
            set => _SimulateTrialMode = value;
        }

    }
}
