using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using SQLite;

namespace TaskManagementLibrary.Models
{
    [Table("Tasks")]
    public class Tasks
    {
        //private static int id = 1;
        [PrimaryKey]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CreatedBy { get; set; }
        public StatusType Status { get; set; }
        public PriorityType Priority { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public Tasks(string name, string desc,string owner, StatusType status, PriorityType type, DateTimeOffset startDate, DateTimeOffset endDate, int projectId)
        {
            ProjectId = projectId;
            Name = name;
            Desc = desc;
            CreatedBy = owner;
            Status = status;
            Priority = type;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Tasks(int id,string name, string desc, string owner, StatusType status, PriorityType type, DateTimeOffset startDate, DateTimeOffset endDate, int projectId)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Desc = desc;
            CreatedBy = owner;
            Status = status;
            Priority = type;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Tasks() { }
        
    }
}
