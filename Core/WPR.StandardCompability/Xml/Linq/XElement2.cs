using System.Xml.Linq;
using System.IO;

namespace WPR.StandardCompability.Xml.Linq
{
    public class XElement2
    {
        public static XElement Load(string path)
        {
            return XElement.Load((Path.DirectorySeparatorChar == '\\') ? path : path.Replace('\\', Path.DirectorySeparatorChar));
        }
    }
}