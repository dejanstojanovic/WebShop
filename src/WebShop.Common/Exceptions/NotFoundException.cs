using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WebShop.Common.Exceptions
{
    [Serializable]
    public class NotFoundException:Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
