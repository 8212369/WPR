using System;

namespace WPR.WindowsCompability
{
    public abstract class Type2
    {
        public static Type? GetType(string typeName, bool throwOnError)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("Type name is null!");
            }

            var stuffs = typeName.Split(',');
            if (stuffs.Length >= 2)
            {
                bool patched = false;
                for (int i = 1; i < stuffs.Length; i += 4)
                {
                    if (stuffs[i].Contains("Microsoft.Xna.Framework"))
                    {
                        if (!stuffs[i].Equals("Microsoft.Xna.Framework.GamerServices"))
                        {
                            stuffs[i] = "FNA";
                            patched = true;
                        }
                    }
                }
                if (patched)
                {
                    typeName = stuffs[0];
                    for (int i = 1; i < stuffs.Length; i += 4)
                    {
                        typeName += $", {stuffs[i]}";
                    }
                }
            }

            return Type.GetType(typeName);
        }
    }
}
