
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
    public class ChangeProjectStatusDataManager : TaskManagementDataManager, IChangeProjectStatusDataManager
    {
        public ChangeProjectStatusDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangeStatusOfProject(ChangeProjectStatusRequest request, IUsecaseCallbackBasecase<ChangeProjectStatusResponse> callback)
        {
            StatusType status = request.status;
            int projectId = request.projectId;
            var project = DBhandler.GetProject(projectId);
            project.Status = status;
            DBhandler.UpdateProject(project);
            ZResponse<ChangeProjectStatusResponse> zResponse = new ZResponse<ChangeProjectStatusResponse>();
            zResponse.Response = "Project's status is updated successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}