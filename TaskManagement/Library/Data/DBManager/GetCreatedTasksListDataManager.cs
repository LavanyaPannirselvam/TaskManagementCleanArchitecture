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
    internal class GetCreatedTasksListDataManager : TaskManagementDataManager , IGetCreatedTasksListDataManager
    {
        public GetCreatedTasksListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetCreatedTasks(GetCreatedTasksListRequest request, IUsecaseCallbackBasecase<GetCreatedTasksListResponse> response)
        {
            var list = DBhandler.CreatedTasksListOfCurrentUser(request.userName, request.userEmail);
            GetCreatedTasksListResponse tasksResponse = new GetCreatedTasksListResponse();
            ZResponse<GetCreatedTasksListResponse> zResponse = new ZResponse<GetCreatedTasksListResponse>();
            if (list.Count > 0)
            {
                tasksResponse.Data = list;
                zResponse.Response = "";
            }
            else
            {
                zResponse.Response = "No task created yet:)";
            }
            zResponse.Data = tasksResponse;
            response.OnResponseSuccess(zResponse);
        }
    } 
}
