using System.Collections;
using System.Collections.Generic;

namespace WPR.XnaCompability.Media
{
    public sealed class ArtistCollection : IEnumerable<Artist>, IEnumerable
    {
        private List<Artist> _Artists;

        internal ArtistCollection()
        {
            _Artists = new List<Artist>();
        }

        public IEnumerator<Artist> GetEnumerator() => _Artists.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Artists.GetEnumerator();
        public int Count => _Artists.Count;
    }
}
