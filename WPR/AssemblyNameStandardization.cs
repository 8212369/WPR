using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPR
{
    internal class AssemblyNameStandardization
    {
        public static String Process(String previous)
        {
            return new Regex("[*'\",_&#^@!]").Replace(previous, "_");
        }
    }
}
