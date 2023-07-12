using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
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
           // Notifications.Notification.UserAssignedUpdate(DbHandler.GetTask(request.taskId));
            callback.OnResponseSuccess(zResponse);
        }
    }
}

