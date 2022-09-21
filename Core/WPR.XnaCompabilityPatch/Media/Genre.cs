using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.XnaCompability.Media
{
    public class Genre
    {
        internal Genre()
        {
            Name = "Unknown";
            Songs = new SongCollection();
        }

        public string Name { get; }
        public SongCollection Songs { get; }
    }
}
