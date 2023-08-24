using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary
{
    public class BaseException : Exception 
    {
        public string exceptionMessage;
        public BaseException(Exception ex)
        {

        }
        public BaseException(String message)
        {
            exceptionMessage = message;
        }
        public BaseException()
        {

        }

    }
}
