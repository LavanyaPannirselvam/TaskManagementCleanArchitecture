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
            var assignedTasksList = DBhandler.AssignedTasksListOfAProject(request.projectId,request.count,request.skipCount);
            GetTasksListResponse taskResponse = new GetTasksListResponse();
            ZResponse<GetTasksListResponse> zResponse = new ZResponse<GetTasksListResponse>();
            if (assignedTasksList.Count > 0)
            {
                zResponse.Response = "";
            }
            else
            {
                zResponse.Response = "Tasks not assigned yet :)";
            }
            taskResponse.Data= assignedTasksList;
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
