using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class LeaderboardEntry
    {
        private Gamer? _Gamer;
        private PropertyDictionary? _Columns;
        private long _Rating;

        public Gamer? Gamer
        {
            get => _Gamer;
            set => _Gamer = value;
        }

        public PropertyDictionary? Columns
        {
            get => _Columns;
            set => _Columns = value;
        }

        public long Rating
        {
            get => _Rating;
            set => _Rating = value;
        }

        public LeaderboardEntry()
        {
            _Columns = new PropertyDictionary(false, 20);
        }
    }
}
