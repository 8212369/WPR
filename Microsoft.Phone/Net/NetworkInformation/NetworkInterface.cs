using System.Net.NetworkInformation;

namespace Microsoft.Phone.Net.NetworkInformation
{
    public sealed class NetworkInterface
    {
        public static NetworkInterfaceType NetworkInterfaceType
        {
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
        }
    }
}
