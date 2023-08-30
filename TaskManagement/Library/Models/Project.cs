using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using SQLite;

namespace TaskManagementLibrary.Models
{
    public class Project
    {
        //private static int id = 9;
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CreatedBy { get; set; }
        public StatusType Status { get; set; }
        public PriorityType Priority { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public Project(string name, string desc,string ownerName, StatusType status, PriorityType type, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            Name = name;
            Desc = desc;
            CreatedBy = ownerName;
            Status = status;
            Priority = type;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Project() { }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            Project obj = other as Project;
            return Id == obj.Id;
        }
    }
}
