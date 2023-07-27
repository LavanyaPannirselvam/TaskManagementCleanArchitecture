using TaskManagementLibrary.Models.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class Assignment
    {
        [PrimaryKey]
        public string UserEmail { get; set; }
        //[PrimaryKey]
        public int ActivityId { get; set; }
        public ActivityType Type { get; set; }
        public Assignment(string userEmail, int activityId,ActivityType type)
        {
            UserEmail = userEmail;
            ActivityId = activityId;
            Type = type;
        }
        public Assignment() { }
    }
}
