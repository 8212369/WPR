using System;

namespace WPR.WindowsCompability
{
    public class GC2
    {
        private static bool IsMono;

        static GC2()
        {
#if __MOBILE__
            IsMono = true;
#else
            IsMono = (Type.GetType("Mono.Runtime") != null);
#endif
        }

        public static void Collect()
        {
            // Top performance killer on Mono. The GC should be a lot better than it was before
            // that developers have to manually insert this...
            if (!IsMono)
            {
                GC.Collect();
            }
        }
    }
}
