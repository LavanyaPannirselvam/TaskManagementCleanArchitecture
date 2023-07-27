using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class IssueBO : Issue
    {
        //public Issue Issue { get; set; }
        //public Dictionary<User, bool> UsersList { get; set; }
        //public IssueBO()
        //{
        //    UsersList = new Dictionary<User, bool>();
        //}
        public ObservableCollection<UserBO> AssignedUsers { get; set; }
        
        public IssueBO() 
        {
            AssignedUsers = new ObservableCollection<UserBO>();
        }

        public IssueBO(Issue issue) : base (issue.Id,issue.Name,issue.Desc,issue.CreatedBy,issue.Status,issue.Priority,issue.StartDate,issue.EndDate,issue.ProjectId)
        {
            AssignedUsers = new ObservableCollection<UserBO>();
        }
    }
}
