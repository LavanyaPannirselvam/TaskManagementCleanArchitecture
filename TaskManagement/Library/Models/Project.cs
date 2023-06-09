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
        private readonly int id = 1;
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CreatedBy { get; set; }
        public StatusType Status { get; set; }
        public PriorityType Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Project(string name, string desc, StatusType status, PriorityType type, DateTime startDate, DateTime endDate)
        {
            Id = id++;
            Name = name;
            Desc = desc;
            Status = status;
            Priority = type;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Project() { }
        
    }
}
//need to implement util for id creation