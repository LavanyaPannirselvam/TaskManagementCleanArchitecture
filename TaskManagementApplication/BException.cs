using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BException : NoUserException
    {
        public string exceptionMessage;
        public BException(Exception ex)
        {

        }
        public BException(String message)
        {
            exceptionMessage = message;
        }
        public BException()
        {

        }

    }

    [Serializable]
    public class NoUserException : Exception
    {
        public NoUserException()
        {
        }

        public NoUserException(string message) : base(message)
        {
        }

        public NoUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
