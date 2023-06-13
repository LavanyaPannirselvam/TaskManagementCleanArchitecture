using TaskManagementLibrary.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IAssignmentHandler
    {
        void AssignActivity(int userId, int activityId,ActivityType type);
        void DeassignActivity(int userId, int activityId, ActivityType type);
    }
}
