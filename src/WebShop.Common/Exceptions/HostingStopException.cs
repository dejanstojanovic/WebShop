using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WebShop.Common.Exceptions
{
    public class HostingStopException : Exception
    {
        public HostingStopException()
        {
        }

        public HostingStopException(string message)
            : base(message)
        {
        }

        public HostingStopException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected HostingStopException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
