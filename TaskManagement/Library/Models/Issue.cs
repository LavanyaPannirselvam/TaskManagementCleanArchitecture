using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Models
{
    [Table("Issues")]
    public class Issue
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Issue(string name, string desc, string owner, StatusType status, PriorityType type, DateTime startDate, DateTime endDate, int projectId)
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

        public Issue(int id,string name, string desc, string owner, StatusType status, PriorityType type, DateTime startDate, DateTime endDate, int projectId)
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
        public Issue() { }
    }
}
