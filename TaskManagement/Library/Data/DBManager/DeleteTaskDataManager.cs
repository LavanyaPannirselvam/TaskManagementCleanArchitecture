
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using static TaskManagementLibrary.Domain.Usecase.DeleteTask;

namespace TaskManagementLibrary.Data.DBManager
{
    public class DeleteTaskDataManager : TaskManagementDataManager , IDeleteTaskDataManager
    {
        public DeleteTaskDataManager(IDBHandler dbHandler) : base(dbHandler) {}

        public void DeleteTask(DeleteTaskRequest request, IUsecaseCallbackBasecase<DeleteTaskResponse> response)
        {
            var id = request.taskId;
            var data = DBhandler.GetTask(id);
            var assignedUsersList = DBhandler.AssignedUsersListOfATask(id);
            if (assignedUsersList != null)
            {
                DBhandler.RemoveAllAssignments(assignedUsersList);
            }
            DBhandler.DeleteTask(id);
            ZResponse<DeleteTaskResponse> zResponse = new ZResponse<DeleteTaskResponse>();
            DeleteTaskResponse deleteTask = new DeleteTaskResponse();
            deleteTask.Data = data;
            zResponse.Response = "Task deleted successfully";
            zResponse.Data = deleteTask;
            response.OnResponseSuccess(zResponse);
        }
    }
}
