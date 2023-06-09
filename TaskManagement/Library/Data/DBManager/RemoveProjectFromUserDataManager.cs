
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
    public class RemoveProjectFromUserDataManager : TaskManagementDataManager,IRemoveProjectFromUserDataManager
    {
        public RemoveProjectFromUserDataManager(IDBHandler dBHandler) : base (dBHandler){ }

        public void RemoveProjectFromUser(RemoveProjectRequest request, IUsecaseCallbackBasecase<RemoveProjectResponse> callback)
        {
            int projectId = request.projectId;
            int userId  = request.userId;
            DbHandler.DeassignActivity(userId, projectId, ActivityType.PROJECT);
            ZResponse<RemoveProjectResponse> zResponse = new ZResponse<RemoveProjectResponse>();
            zResponse.Response = "Project removed from user successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
