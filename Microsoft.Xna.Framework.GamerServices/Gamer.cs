using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return Task.Run(async () =>
            {
                GamerProfile profile = new GamerProfile();
                var earnedIte = AchievementContext.Current.Achievements!.Where(a => a.IsEarned);

                profile.TotalAchievements = await earnedIte.CountAsync();
                profile.GamerScore = await earnedIte.SumAsync(a => a.GamerScore);
                profile.GamerZone = GamerZone.Underground;
                profile.Region = System.Globalization.RegionInfo.CurrentRegion;
                profile.Reputation = 100.0f;
                profile.Motto = "";

                if (callback != null)
                {
                    TaskCompletionSource<GamerProfile> source = new TaskCompletionSource<GamerProfile>(asyncState);
                    source.SetResult(profile);

                    callback(source.Task);
                }

                return profile;
            });
        }

        public GamerProfile EndGetProfile(IAsyncResult result)
        {
            return (result as Task<GamerProfile>)!.Result;
        }

        public GamerProfile GetProfile() => EndGetProfile(BeginGetProfile(null, null));

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

        public string DisplayName => _GamerTag;

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
