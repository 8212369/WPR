using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework.GamerServices
{
    [Serializable]
    public class GamerPrivilegeException : Exception
    {
        public GamerPrivilegeException()
        {
        }

        public GamerPrivilegeException(string message)
            : base(message)
        {
        }

        protected GamerPrivilegeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GamerPrivilegeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
