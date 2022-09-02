using DesktopNotifications;
using DesktopNotifications.Apple;
using DesktopNotifications.FreeDesktop;
using DesktopNotifications.Windows;

using System.Runtime.InteropServices;
using System;

namespace WPR.Common
{
    public static class NativeUI
    {
        public static INotificationManager NotificationManager { get; set; }

        static NativeUI()
        {
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
            } else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
