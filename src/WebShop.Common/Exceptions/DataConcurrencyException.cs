using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Common.Exceptions
{
    public class DataConcurrencyException : Exception
    {
        public DataConcurrencyException():base()
        {

        }
        public DataConcurrencyException(String message, Exception innerException):base(message, innerException)
        {

        }
    }
}
