using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class GamerCollection<T> : ReadOnlyCollection<T>, IEnumerable<Gamer>, IEnumerable where T : Gamer
    {

        public struct GamerCollectionEnumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            public T Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        internal GamerCollection()
            : base(new List<T>())
        {
        }

        IEnumerator<Gamer> IEnumerable<Gamer>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public GamerCollectionEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}
