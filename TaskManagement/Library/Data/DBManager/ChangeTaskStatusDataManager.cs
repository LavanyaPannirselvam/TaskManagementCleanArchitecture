
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class ChangeTaskStatusDataManager : TaskManagementDataManager, IChangeTaskStatusDataManager
    {
        public ChangeTaskStatusDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangeStatusOfTask(ChangeTaskStatusRequest request, IUsecaseCallbackBasecase<ChangeTaskStatusResponse> callback)
        {
            StatusType status = request.status;
            var task = DBhandler.GetTask(request.taskId);
            task.Status = status;
            DBhandler.UpdateTask(task);
            ZResponse<ChangeTaskStatusResponse> zResponse = new ZResponse<ChangeTaskStatusResponse>();
            ChangeTaskStatusResponse response = new ChangeTaskStatusResponse();
            response.Data = true;
            zResponse.Response = "Task's status updated";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}