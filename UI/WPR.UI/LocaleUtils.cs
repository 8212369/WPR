using System;
using WPR.UI.Properties;

namespace WPR.UI
{
    public static class LocaleUtils
    {
        public static string GetDisplayName(this Enum e)
        {
            var resourceDisplayName = Resources.ResourceManager.GetString(e.GetType().Name + "_" + e);
            return string.IsNullOrWhiteSpace(resourceDisplayName) ? string.Format("[[{0}]]", e) :
                resourceDisplayName;
        }
    }
}
