using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Tasks
{
    public class MediaPlayerLauncher
    {
        public MediaLocationType Location { get; set; }

        public Uri Media { get; set; }

        public MediaPlaybackControls Controls { get; set; }

        public MediaPlayerOrientation Orientation { get; set; }

        public void Show()
        {

        }
    }
}
