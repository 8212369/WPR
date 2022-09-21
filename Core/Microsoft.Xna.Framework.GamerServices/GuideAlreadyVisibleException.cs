using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework.GamerServices
{
    [Serializable]
    public class GuideAlreadyVisibleException : Exception
    {
        public GuideAlreadyVisibleException()
        {
        }

        public GuideAlreadyVisibleException(string message)
            : base(message)
        {
        }

        protected GuideAlreadyVisibleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GuideAlreadyVisibleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }  
}
