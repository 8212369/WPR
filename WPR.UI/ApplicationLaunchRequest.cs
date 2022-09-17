using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.UI
{
    public class ApplicationLaunchRequestArgs : EventArgs
    {
        internal ApplicationLaunchRequestArgs(Models.Application app)
        {
            this.Target = app;
        }

        public Models.Application Target { get; set; }
    }

    public static class ApplicationLaunchRequest
    {
        public static EventHandler<ApplicationLaunchRequestArgs>? Incoming;

        public static void Ask(Models.Application app)
        {
            Incoming?.Invoke(null, new ApplicationLaunchRequestArgs(app));
        }
    }
}
