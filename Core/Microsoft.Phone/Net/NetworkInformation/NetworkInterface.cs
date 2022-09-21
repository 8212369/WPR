using System.Net.NetworkInformation;

namespace Microsoft.Phone.Net.NetworkInformation
{
    public sealed class NetworkInterface
    {
        public static NetworkInterfaceType NetworkInterfaceType
        {
            // Android currently with .NET6 is incompatible with API level 30 on getifaddrs
            // Supply a constant for now!
#if __ANDROID__
            get {
                return NetworkInterfaceType.Wireless80211;
            }
#else
            get
            {
                foreach (System.Net.NetworkInformation.NetworkInterface netInterface in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        return (NetworkInterfaceType)netInterface.NetworkInterfaceType;
                    }
                }

                return NetworkInterfaceType.None;
            }
#endif
        }
    }
}
