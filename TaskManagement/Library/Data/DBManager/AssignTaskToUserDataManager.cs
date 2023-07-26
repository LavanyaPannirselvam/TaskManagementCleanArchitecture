using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            DBhandler.AssignActivity(userId,taskId,ActivityType.TASK);
            ZResponse<AssignTaskResponse> zResponse = new ZResponse<AssignTaskResponse>();
            //zResponse.Response = "Task assigned to user successfully";
           // List<User> users = DbHandler.UsersList();
            List<User> assignedUsers = DBhandler.AssignedUsersList(request.taskId, (int)ActivityType.TASK);
            AssignTaskResponse assignTaskResponse = new AssignTaskResponse();
            //foreach(var u in users)
            //{
            //    if (assignedUsers.Contains<User>(u))
            //        assignTaskResponse.users.Add(u);
            //}
            assignTaskResponse.Data = new ObservableCollection<User>(assignedUsers); 
            zResponse.Response = "Task assigned to user successfully";
            zResponse.Data = assignTaskResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}

