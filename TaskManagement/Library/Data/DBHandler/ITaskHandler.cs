using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface ITaskHandler
    {
        int AddTask(Tasks task);
        void DeleteTask(int taskId);
        Tasks GetTask(int taskId);
        List<Tasks> TasksList();
        List<Tasks> AssignedTasksListOfCurrentUser(string userEmail);
        List<Tasks> CreatedTasksListOfCurrentUser(string userName, string userEmail);
        List<Assignment> AssignedUsersListOfATask(int taskId);
        void UpdateTask(Tasks task);
    }
}
