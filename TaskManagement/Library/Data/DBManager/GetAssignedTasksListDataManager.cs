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

namespace TaskManagementLibrary.Data.DBManager
{
    public class GetAssignedTasksListDataManager : TaskManagementDataManager , IGetAssignedTasksListDataManager
    {
        public GetAssignedTasksListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetAssignedTasks(GetAssignedTasksListRequest request, IUsecaseCallbackBasecase<GetAssignedTasksListResponse> response)
        {
            var list = DBhandler.AssignedTasksListOfCurrentUser(request.userEmail);
            GetAssignedTasksListResponse tasksResponse = new GetAssignedTasksListResponse();
            ZResponse<GetAssignedTasksListResponse> zResponse = new ZResponse<GetAssignedTasksListResponse>();
            if (list.Count > 0)
            {
                tasksResponse.Data = new ObservableCollection<Tasks>(list);
                zResponse.Response = "";
            }
            else
            {
                tasksResponse.Data = null;
                zResponse.Response = "No task assigned yet:)";
            }
            zResponse.Data = tasksResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
