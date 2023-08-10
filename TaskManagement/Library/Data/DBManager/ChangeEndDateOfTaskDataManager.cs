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
    public class ChangeEndDateOfTaskDataManager : TaskManagementDataManager, IChangeEndDateofTaskDataManager
    {
        public ChangeEndDateOfTaskDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeEndDate(ChangeEndDateofTaskRequest request, IUsecaseCallbackBasecase<ChangeEndDateofTaskResponse> callback)
        {
            var task = DBhandler.GetTask(request.taskId);
            task.EndDate = request.date;
            DBhandler.UpdateTask(task);
            ChangeEndDateofTaskResponse response = new ChangeEndDateofTaskResponse();
            ZResponse<ChangeEndDateofTaskResponse> zResponse = new ZResponse<ChangeEndDateofTaskResponse>();
            response.Data = DBhandler.GetTask(request.taskId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
