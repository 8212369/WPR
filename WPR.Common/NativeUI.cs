using DesktopNotifications;
using DesktopNotifications.Apple;
using DesktopNotifications.FreeDesktop;
using DesktopNotifications.Windows;

#if __ANDROID__
using DesktopNotifications.Android;
#endif

using System.Runtime.InteropServices;
using System;

namespace WPR.Common
{
    public static class NativeUI
    {
        public static INotificationManager NotificationManager { get; set; }

        public static void Initialize(object hostControl = null)
        {
#if __ANDROID__
            NotificationManager = new AndroidNotificationManager((hostControl as Android.Content.Context)!);
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                NotificationManager = new FreeDesktopNotificationManager();
            } 
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NotificationManager = new WindowsNotificationManager();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                NotificationManager = new AppleNotificationManager();
            } else {
                throw new PlatformNotSupportedException();
            }
#endif
            NotificationManager.Initialize();
        }
    }
}
