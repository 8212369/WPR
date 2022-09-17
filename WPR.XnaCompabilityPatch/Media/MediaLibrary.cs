using System;
using System.Collections.Generic;

namespace WPR.XnaCompability.Media
{
    public class MediaLibrary
    {
        private SongCollection _Songs;
        private ArtistCollection _Artists;
        private AlbumCollection _Albums;

        public MediaLibrary(MediaSource source)
        {
            _Songs = new SongCollection();
            _Artists = new ArtistCollection();
            _Albums = new AlbumCollection();
        }

        public SongCollection Songs => _Songs;
        public ArtistCollection Artists => _Artists;
        public AlbumCollection Albums => _Albums;
    }
}
