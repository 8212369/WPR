using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Media;

namespace WPR.XnaCompability.Media
{
    public sealed class SongCollection : IEnumerable<Song>, IEnumerable
    {
        private List<Song> _Songs;

        internal SongCollection()
        {
            _Songs = new List<Song>();
        }

        public IEnumerator<Song> GetEnumerator() => _Songs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Songs.GetEnumerator();
        public int Count => _Songs.Count;
    }
}
