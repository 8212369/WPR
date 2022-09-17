using System.Collections;
using System.Collections.Generic;

namespace WPR.XnaCompability.Media
{
    public sealed class AlbumCollection : IEnumerable<Album>, IEnumerable
    {
        private List<Album> _Albums;

        internal AlbumCollection()
        {
            _Albums = new List<Album>();
        }

        public IEnumerator<Album> GetEnumerator() => _Albums.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Albums.GetEnumerator();

        public int Count => _Albums.Count;
    }
}
