using System;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class LeaderboardWriter
    {
        private LeaderboardEntry? _Entry;

        public LeaderboardWriter()
        {
            _Entry = new LeaderboardEntry();
        }

        public LeaderboardEntry GetLeaderboard(LeaderboardIdentity identity)
        {
            return _Entry;
        }
    }
}
