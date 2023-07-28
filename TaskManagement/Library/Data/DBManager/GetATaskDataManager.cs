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
    public class GetATaskDataManager : TaskManagementDataManager, IGetATaskDataManager
    {
        public GetATaskDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetATask(GetATaskRequest request, IUsecaseCallbackBasecase<GetATaskResponse> response)
        {
            var item = DBhandler.GetTask(request.taskId);
            TaskBO taskBO = new TaskBO(item);
            List<UserBO> list= DBhandler.AssignedUsersList(request.taskId, (int)ActivityType.TASK);
            //List<User> userList = DbHandler.UsersList();
            ZResponse<GetATaskResponse> zResponse = new ZResponse<GetATaskResponse>();
            GetATaskResponse taskResponse = new GetATaskResponse();
            //if (list.Count > 0)
            //{
            //    //taskBO.AssignedUsers = list;
            //    foreach (var user in userList)
            //    {
            //        if (list.Contains<User>(user))
            //            taskBO.UsersList.Add(user, true);
            //        else
            //            taskBO.UsersList.Add(user, false);
            //    }
            //    zResponse.Response = "";
            //}
            //else
            //{
            //    zResponse.Response = "Users not assigned yet :)";
            //    foreach (var user in userList)
            //            taskBO.UsersList.Add(user, false);            
            //}
            if (list.Count == 0)
            {
                zResponse.Response = "Users not assigned yet :)";
            }
            taskBO.AssignedUsers = list;
            taskResponse.Data = taskBO;
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}

