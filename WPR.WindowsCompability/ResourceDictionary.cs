using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPR.WindowsCompability
{
    public class ResourceDictionary : Dictionary<string, object?>
    {
        public bool Contains(object obj)
        {
            return base.ContainsKey((obj as string)!);
        }

        public object this[object obj]
        {
            get => base[(obj as string)!];
            set => base[(obj as string)!] = value;
        }
    }
}
