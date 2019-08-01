using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WebShop.Common.Exceptions
{
    public class MismatchException:Exception
    {
        public MismatchException()
        {
        }

        public MismatchException(string message)
            : base(message)
        {
        }

        public MismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected MismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
