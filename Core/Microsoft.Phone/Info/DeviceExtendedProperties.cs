using System;

namespace Microsoft.Phone.Info
{
    public class DeviceExtendedProperties
    {
        public static bool TryGetValue(string propertyName, out Object propertyValue)
        {
            propertyValue = GetValue(propertyName);
            return true;
        }

        public static Object? GetValue(string property)
        {
            switch (property)
            {
                case "DeviceManufacturer":
                    return "WPRunner";

                case "DeviceName":
                    return "WPRunner 2022";

                case "DeviceFirmwareVersion":
                case "DeviceHardwareVersion":
                    return "8.0.0";

                case "DeviceTotalMemory":
                    // Return 2GB RAM
                    return 2048L * 1024 * 1024;

                default:
                    return null;
            }
        }
    }
}
