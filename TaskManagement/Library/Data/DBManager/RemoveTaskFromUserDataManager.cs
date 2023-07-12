
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
    public class RemoveTaskFromUserDataManager : TaskManagementDataManager,IRemoveTaskFromUserDataManager
    {
        public RemoveTaskFromUserDataManager(IDBHandler dBHandler) : base (dBHandler){ }

        public void RemoveTaskFromUser(RemoveTaskRequest request, IUsecaseCallbackBasecase<RemoveTaskResponse> callback)
        {
            int taskId = request.taskId;
            int userId  = request.userId;
            DbHandler.DeassignActivity(userId, taskId, ActivityType.TASK);
            ZResponse<RemoveTaskResponse> zResponse = new ZResponse<RemoveTaskResponse>();
            zResponse.Response = "Task removed from user successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
