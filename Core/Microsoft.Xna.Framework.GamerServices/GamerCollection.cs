using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class GamerCollection<T> : ReadOnlyCollection<T>, IEnumerable<Gamer>, IEnumerable where T : Gamer
    {
        private List<T> _InternalList;

        public struct GamerCollectionEnumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            internal GamerCollectionEnumerator(List<T>.Enumerator enumerator)
            {
                _Enumerator = enumerator;
            }

            private List<T>.Enumerator _Enumerator;

            public T Current => _Enumerator.Current;

            public void Dispose() => _Enumerator.Dispose();

            void IEnumerator.Reset()
            {
                (_Enumerator as IEnumerator).Reset();
            }

            public bool MoveNext() => _Enumerator.MoveNext();

            object IEnumerator.Current => _Enumerator.Current;
        }

        internal GamerCollection()
            : base(new List<T>())
        {
            _InternalList = (Items as List<T>)!;
        }

        public GamerCollection(List<T> list)
            : base(list)
        {
            _InternalList = (Items as List<T>)!;
        }

        IEnumerator<Gamer> IEnumerable<Gamer>.GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        public new GamerCollectionEnumerator GetEnumerator()
        {
            return new GamerCollectionEnumerator(_InternalList.GetEnumerator());
        }

    }
}
