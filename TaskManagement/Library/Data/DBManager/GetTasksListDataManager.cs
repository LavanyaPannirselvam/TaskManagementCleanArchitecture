using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;

namespace TaskManagementLibrary.Data.DBManager
{
    public class GetTasksListDataManager : TaskManagementDataManager, IGetTasksListDataManager
    {
        public GetTasksListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetTasks(GetTasksListRequest request, IUsecaseCallbackBasecase<GetTasksListResponse> response)
        {
            var list = DbHandler.AssignedTasksList(request.projectId);
            GetTasksListResponse taskResponse = new GetTasksListResponse();
            taskResponse.Tasks = list;
            ZResponse<GetTasksListResponse> zResponse = new ZResponse<GetTasksListResponse>();
            zResponse.Response = "";
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
