using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.Common.Dtos
{
    /// <summary>
    /// Error message details
    /// </summary>
    public class ErrorMessageDto
    {
        private readonly Exception exception;

        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="ex"></param>
        public ErrorMessageDto(Exception ex)
        {
            this.exception = ex;
        }

        /// <summary>
        /// Detailed error message which occured on the server
        /// </summary>
        public String Error
        {
            get
            {
                return exception.Message;
            }
        }

        /// <summary>
        /// Server exception stack trace
        /// </summary>
        public String Trace
        {
            get
            {
                return exception.StackTrace;
            }
        }

    }
}
