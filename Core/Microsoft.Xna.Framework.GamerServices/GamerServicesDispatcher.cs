using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class GamerServicesDispatcher
    {

        public static event EventHandler<EventArgs> InstallingTitleUpdate;


        public static void Initialize(IServiceProvider serviceProvider)
        {
        }

        public static void Update()
        {
        }

        public static bool IsInitialized => true;

        public static IntPtr WindowHandle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
