using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Errors
{
   public class RestException: Exception
    {
        public RestException(HttpStatusCode httpStatusCode, Object error = null )
        {
            HttpStatusCode = httpStatusCode;
            Error = error;
        }

        public HttpStatusCode HttpStatusCode { get; }
        public object Error { get; }
    }
}
