using System;
using System.Collections.Generic;
using System.Text;

using WPR.Common;

namespace Microsoft.Xna.Framework.GamerServices
{
    public abstract class Gamer
    {
        internal static SignedInGamerCollection _SignedInGamers;

        private LeaderboardWriter _LeaderboardWriter;
        private String _GamerTag;

        internal Gamer()
        {
            _LeaderboardWriter = new LeaderboardWriter();
            _GamerTag = Configuration.Current.GamerTag ?? "HarryDirk";
        }

        static Gamer()
        {
            _SignedInGamers = new SignedInGamerCollection(new List<SignedInGamer>{
                new SignedInGamer() { PlayerIndex = PlayerIndex.One }
            });
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

        public static IAsyncResult BeginGetPartnerToken(
          string audienceUri,
          AsyncCallback callback,
          object asyncState)
        {
            return StubUtils.ForeverTask;
        }

        public string Gamertag
        {
            get => _GamerTag;
            set => _GamerTag = value;
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
                return _SignedInGamers;
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

        public LeaderboardWriter LeaderboardWriter
        {
            get => _LeaderboardWriter;
        }
    }
}
