using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.Common
{
    public static class StubUtils
    {
        private static IAsyncResult _ForeverTask = Task.Run(() => {
            while (true) ;
        });

        public static IAsyncResult ForeverTask => _ForeverTask;
    }
}
