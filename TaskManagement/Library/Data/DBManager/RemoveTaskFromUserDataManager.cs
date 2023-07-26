
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
    public class RemoveTaskFromUserDataManager : TaskManagementDataManager,IRemoveTaskFromUserDataManager
    {
        public RemoveTaskFromUserDataManager(IDBHandler dBHandler) : base (dBHandler){ }

        public void RemoveTaskFromUser(RemoveTaskRequest request, IUsecaseCallbackBasecase<RemoveTaskResponse> callback)
        {
            DBhandler.DeassignActivity(request.userId, request.taskId, ActivityType.TASK);
            ZResponse<RemoveTaskResponse> zResponse = new ZResponse<RemoveTaskResponse>();
            zResponse.Response = "Task removed from user successfully";
            RemoveTaskResponse removeTaskResponse = new RemoveTaskResponse();
            //List<User> users = DbHandler.UsersList();
            List<User> assignedUsers = DBhandler.AssignedUsersList(request.taskId, (int)ActivityType.TASK);
            //foreach (var u in users)
            //{
            //    if (!assignedUsers.Contains<User>(u))
            //        removeTaskResponse.users.Add(u);
            //}
            removeTaskResponse.Data = new System.Collections.ObjectModel.ObservableCollection<User>(assignedUsers);
            zResponse.Data = removeTaskResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
