using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;

namespace TaskManagementLibrary.Data.DBManager
{
    public class GetTaskListDataManager : TaskManagementDataManager, IGetTasksListDataManager
    {
        public GetTaskListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetTasks(GetTasksListRequest request, IUsecaseCallbackBasecase<GetTasksListResponse> response)
        {
            var item = DbHandler.AssignedTasksList(request.projectId);
            GetTasksListResponse taskResponse = new GetTasksListResponse();
            taskResponse.Tasks= item;
            ZResponse<GetTasksListResponse> zResponse = new ZResponse<GetTasksListResponse>();
            zResponse.Response = "";
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
