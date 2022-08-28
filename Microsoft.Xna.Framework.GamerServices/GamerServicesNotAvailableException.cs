using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework.GamerServices
{
    [Serializable]
    public class GamerServicesNotAvailableException : Exception
    {
        public GamerServicesNotAvailableException()
        {
        }

        public GamerServicesNotAvailableException(string message)
            : base(message)
        {
        }

        protected GamerServicesNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GamerServicesNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
