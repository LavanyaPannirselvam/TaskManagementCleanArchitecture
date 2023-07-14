
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
    public class DeleteTaskDataManager : TaskManagementDataManager , IDeleteTaskDataManager
    {
        public DeleteTaskDataManager(IDBHandler dbHandler) : base(dbHandler) {}

        public void DeleteTask(DeleteTaskRequest request, IUsecaseCallbackBasecase<bool> response)
        {
            var id = request.taskId;
            DbHandler.DeleteTask(id);
            ZResponse<bool> zResponse = new ZResponse<bool>();
            zResponse.Response = "Task deleted successfully";
            zResponse.Data = true;
            response.OnResponseSuccess(zResponse);
        }
    }
    //event to be created and handled
}
