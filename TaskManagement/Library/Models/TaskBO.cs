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
        public List<User> AssignedUsers { get; set; }
    }
}
