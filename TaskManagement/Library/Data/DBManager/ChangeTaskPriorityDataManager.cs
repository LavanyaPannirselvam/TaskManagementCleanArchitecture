using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class ChangeTaskPriorityDataManager : TaskManagementDataManager, IChangeTaskPriorityDataManager
    {
        public ChangeTaskPriorityDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangePriorityOfTask(ChangeTaskPriorityRequest request, IUsecaseCallbackBasecase<ChangeTaskPriorityResponse> callback)
        {
            PriorityType priorityType = request.priority;
            int taskId = request.taskId;
            var project = DBhandler.GetTask(taskId);
            project.Priority = priorityType;
            DBhandler.UpdateTask(project);
            ZResponse<ChangeTaskPriorityResponse> zResponse = new ZResponse<ChangeTaskPriorityResponse>();
            ChangeTaskPriorityResponse response = new ChangeTaskPriorityResponse();
            response.Data = true;
            zResponse.Response = "Task's priority is updated successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
