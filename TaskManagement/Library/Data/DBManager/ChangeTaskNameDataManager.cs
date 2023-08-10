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
    public class ChangeTaskNameDataManager : TaskManagementDataManager, IChangeTaskNameDataManager
    {
        public ChangeTaskNameDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeName(ChangeTaskNameRequest request, IUsecaseCallbackBasecase<ChangeTaskNameResponse> callback)
        {
            var task = DBhandler.GetTask(request.taskId);
            task.Name = request.name;
            DBhandler.UpdateTask(task);
            ChangeTaskNameResponse response = new ChangeTaskNameResponse();
            ZResponse<ChangeTaskNameResponse> zResponse = new ZResponse<ChangeTaskNameResponse>();
            response.Data = DBhandler.GetTask(request.taskId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
