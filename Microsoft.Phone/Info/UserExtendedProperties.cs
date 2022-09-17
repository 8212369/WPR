using System;

namespace Microsoft.Phone.Info
{
    public static class UserExtendedProperties
    {
        public static Object GetValue(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("Null property name in retrieving user extended properties' value!");
            }

            switch (propertyName)
            {
                case "ANID":
                    return "123456789";

                default:
                    throw new ArgumentException("Unknown property name!");
            }
        }

        public static bool TryGetValue(string propertyName, out Object propertyValue)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("Null property name in retrieving user extended properties' value!");
            }

            propertyValue = null;

            try
            {
                propertyValue = GetValue(propertyName);
            } catch
            {
                return false;
            }

            return true;
        }
    }
}
