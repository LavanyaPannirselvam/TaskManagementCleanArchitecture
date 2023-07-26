using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class TaskBO : Tasks
    {
        //public Tasks Tasks { get; set; }        
        //public Dictionary<User,bool> UsersList { get; set; }

        public List<User> AssignedUsers { get; set; }

        public TaskBO() 
        {
            AssignedUsers = new List<User>();
        }

        public TaskBO(Tasks tasks) : base(tasks.Name,tasks.Desc,tasks.CreatedBy,tasks.Status,tasks.Priority,tasks.StartDate,tasks.EndDate,tasks.ProjectId) 
        {
            AssignedUsers = new List<User>();
        }

    }
}
