using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GamerProfile : IDisposable
    {

        internal GamerProfile()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Texture2D GamerPicture
        {
            get;
            internal set;
        }

        public int GamerScore
        {
            get;
            internal set;
        }

        public GamerZone GamerZone
        {
            get;
            internal set;
        }

        public bool IsDisposed
        {
            get;
            internal set;
        }

        public string Motto
        {
            get;
            internal set;
        }

        public RegionInfo Region
        {
            get;
            internal set;
        }

        public float Reputation
        {
            get;
            internal set;
        }

        public int TitlesPlayed
        {
            get;
            internal set;
        }

        public int TotalAchievements
        {
            get;
            internal set;
        }

    }
}
