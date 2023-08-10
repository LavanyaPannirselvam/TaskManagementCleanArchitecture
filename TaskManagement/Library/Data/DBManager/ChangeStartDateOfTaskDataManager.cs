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
    public class ChangeStartDateOfTaskDataManager : TaskManagementDataManager, IChangeStartDateofTaskDataManager
    {
        public ChangeStartDateOfTaskDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeStartDate(ChangeStartDateofTaskRequest request, IUsecaseCallbackBasecase<ChangeStartDateofTaskResponse> callback)
        {
            var task = DBhandler.GetTask(request.taskId);
            task.StartDate = request.date;
            DBhandler.UpdateTask(task);
            ChangeStartDateofTaskResponse response = new ChangeStartDateofTaskResponse();
            ZResponse<ChangeStartDateofTaskResponse> zResponse = new ZResponse<ChangeStartDateofTaskResponse>();
            response.Data = DBhandler.GetTask(request.taskId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }

    }
}
