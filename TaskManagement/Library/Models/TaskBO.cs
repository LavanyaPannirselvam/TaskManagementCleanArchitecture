using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class TaskBO
    {
        public Tasks Tasks { get; set; }
        //public List<User> AssignedUsers { get; set; }
        public Dictionary<User,bool> UsersList { get; set; }

        public TaskBO() 
        {
            UsersList = new Dictionary<User,bool>();
        }

    }
}
