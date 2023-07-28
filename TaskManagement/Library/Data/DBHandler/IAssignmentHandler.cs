using TaskManagementLibrary.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IAssignmentHandler
    {
        void AssignActivity(string userEmail, int activityId,ActivityType type);
        void DeassignActivity(string userEmail, int activityId, ActivityType type);
        void RemoveAllAssignments(List<Assignment> assignments);
    }
}
