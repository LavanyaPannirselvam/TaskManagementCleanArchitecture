using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class AssignProjectToUserDataManager : TaskManagementDataManager, IAssignProjectToUserDataManager
    {
        public AssignProjectToUserDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void AssignProject(AssignProjectRequest request, IUsecaseCallbackBasecase<AssignProjectResponse> callback)
        {
            
            int projectId = request.projectId;
            int userId = request.userId;
            DbHandler.AssignActivity(userId,projectId,ActivityType.PROJECT);
            ZResponse<AssignProjectResponse> zResponse = new ZResponse<AssignProjectResponse>();
            zResponse.Response = "Project assigned to user successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}

