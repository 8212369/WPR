using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.XnaCompability.Media
{
    public class Album
    {
        public Artist Artist { get; }
        public TimeSpan Duration { get; }
        public Genre Genre { get; }
        public bool HasArt => false;
        public string Name { get; }
        public SongCollection Songs { get; }

        internal Album()
        {
            Artist = new Artist();
            Songs = new SongCollection();
            Genre = new Genre();
            Duration = new TimeSpan(0, 3, 0);
            Name = "Unknown";
        }
    }
}
