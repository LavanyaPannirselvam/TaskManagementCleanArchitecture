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
            var item = DbHandler.GetTask(request.taskId);
            TaskBO taskBO = new TaskBO();
            taskBO.Tasks = item;
            taskBO.AssignedUsers = DbHandler.AssignedUsersList(request.taskId,(int)ActivityType.TASK);
            GetATaskResponse taskResponse = new GetATaskResponse();
            taskResponse.task = taskBO;
            ZResponse<GetATaskResponse> zResponse = new ZResponse<GetATaskResponse>();
            zResponse.Response = "";
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}

