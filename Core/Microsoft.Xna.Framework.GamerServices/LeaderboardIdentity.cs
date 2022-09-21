
namespace Microsoft.Xna.Framework.GamerServices
{
    public struct LeaderboardIdentity
    {
        public string Key { get; set; }

        public int GameMode { get; set; }

        public static LeaderboardIdentity Create(LeaderboardKey key, int gameMode) => new LeaderboardIdentity()
        {
            Key = key.ToString(),
            GameMode = gameMode
        };

        public static LeaderboardIdentity Create(LeaderboardKey key) => Create(key, 0);
    }
}
