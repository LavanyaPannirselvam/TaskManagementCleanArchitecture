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
    public class ChangeProjectPriorityDataManager : TaskManagementDataManager, IChangeProjectPriorityDataManager
    {
        public ChangeProjectPriorityDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangePriorityOfProject(ChangeProjectPriorityRequest request, IUsecaseCallbackBasecase<ChangeProjectPriorityResponse> callback)
        {
            PriorityType priorityType = request.priority;
            int projectId = request.projectId;
            var project = DBhandler.GetProject(projectId);
            project.Priority = priorityType;
            DBhandler.UpdateProject(project);
            ZResponse<ChangeProjectPriorityResponse> zResponse = new ZResponse<ChangeProjectPriorityResponse>();
            zResponse.Response = "Project's priority is updated successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);

        }
    }
}
