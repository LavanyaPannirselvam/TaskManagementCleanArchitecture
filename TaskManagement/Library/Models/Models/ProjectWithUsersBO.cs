using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class ProjectWithUsersBO
    {
        public Project Project { get; set; }
        public List<User> AssignedUsers { get; set; }
        public ProjectWithUsersBO(Project p,List<User> u) 
        {
            Project = p;
            AssignedUsers = u;
        }

        public ProjectWithUsersBO()
        {
        }
    }
}
