using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class AssignTaskToUserDataManager : TaskManagementDataManager, IAssignTaskToUserDataManager
    {
        public AssignTaskToUserDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void AssignTask(AssignTaskRequest request, IUsecaseCallbackBasecase<AssignTaskResponse> callback)
        {
            int taskId = request.taskId;
            int userId = request.userId;
            DbHandler.AssignActivity(userId,taskId,ActivityType.TASK);
            ZResponse<AssignTaskResponse> zResponse = new ZResponse<AssignTaskResponse>();
            zResponse.Response = "Task assigned to user successfully";
            zResponse.Data = null;
            ZResponse<GetATaskResponse> response = new ZResponse<GetATaskResponse>();
            TaskBO taskBo = new TaskBO();
            taskBo.Tasks = DbHandler.GetTask(taskId);
            List<User> list = DbHandler.AssignedUsersList(request.taskId, (int)ActivityType.TASK);
            List<User> userList = DbHandler.UsersList();
            if (list.Count > 0)
            {
                //taskBO.AssignedUsers = list;
                foreach (var user in userList)
                {
                    if (list.Contains<User>(user))
                        taskBo.UsersList.Add(user, true);
                    else
                        taskBo.UsersList.Add(user, false);
                }
                response.Response = "";
            }
            else
            {
                response.Response = "Users not assigned yet :)";
                foreach (var user in userList)
                    taskBo.UsersList.Add(user, false);
            }
            Notifications.Notification.TaskUserUpdate(taskBo);
            callback.OnResponseSuccess(zResponse);
        }
    }
}

