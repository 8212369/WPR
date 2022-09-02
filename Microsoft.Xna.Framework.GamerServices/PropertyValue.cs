using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.GamerServices
{
    internal abstract class PropertyValue
    {
        public bool IsChanged;
        public bool IsReadOnly = true;

        public abstract object GetValue();

        public abstract void SetValue(object value);
    }
}
