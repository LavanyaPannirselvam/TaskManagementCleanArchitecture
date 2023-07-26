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
    public class GetAssignedTasksListDataManager : TaskManagementDataManager , IGetAssignedTasksListDataManager
    {
        public GetAssignedTasksListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetAssignedTasks(GetAssignedTasksListRequest request, IUsecaseCallbackBasecase<GetAssignedTasksListResponse> response)
        {
            var list = DBhandler.AssignedTasksListOfCurrentUser(request.userId);
            GetAssignedTasksListResponse tasksResponse = new GetAssignedTasksListResponse();
            ZResponse<GetAssignedTasksListResponse> zResponse = new ZResponse<GetAssignedTasksListResponse>();
            if (list.Count > 0)
            {
                tasksResponse.Data = list;
                zResponse.Response = "";
            }
            else
            {
                zResponse.Response = "No task assigned yet:)";
            }
            zResponse.Data = tasksResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
