namespace WPR.XnaCompability.Media
{
    public class Artist
    {
        private SongCollection _Songs;
        private AlbumCollection _Albums;

        internal Artist()
        {
            _Songs = new SongCollection();
            _Albums = new AlbumCollection();
        }

        public string? Name { get; }
        public SongCollection Songs => _Songs;
        public AlbumCollection Albums => Albums;
    }
}
