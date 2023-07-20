using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture
{
    public static class CurrentUserClass
    {
        private static LoggedInUserBO _currentUser;
        public static LoggedInUserBO CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
    }
}
