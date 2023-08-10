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
    public class ChangeTaskDescriptionDataManager : TaskManagementDataManager, IChangeTaskDescriptionDataManager
    {
        public ChangeTaskDescriptionDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeDescription(ChangeTaskDescriptionRequest request, IUsecaseCallbackBasecase<ChangeTaskDescriptionResponse> callback)
        {
            var task = DBhandler.GetTask(request.taskId);
            task.Desc = request.description;
            DBhandler.UpdateTask(task);
            ChangeTaskDescriptionResponse response = new ChangeTaskDescriptionResponse();
            ZResponse<ChangeTaskDescriptionResponse> zResponse = new ZResponse<ChangeTaskDescriptionResponse>();
            response.Data = DBhandler.GetTask(request.taskId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
