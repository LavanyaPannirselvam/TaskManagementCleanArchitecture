using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class ChangeIssueStatusDataManager : TaskManagementDataManager, IChangeIssueStatusDataManager
    {
        public ChangeIssueStatusDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangeStatusOfIssue(ChangeIssueStatusRequest request, IUsecaseCallbackBasecase<ChangeIssueStatusResponse> callback)
        {
            StatusType status = request.status;
            var task = DBhandler.GetTask(request.issueId);
            task.Status = status;
            DBhandler.UpdateTask(task);
            ZResponse<ChangeIssueStatusResponse> zResponse = new ZResponse<ChangeIssueStatusResponse>();
            ChangeIssueStatusResponse response = new ChangeIssueStatusResponse();
            response.Data = true;
            zResponse.Response = "Issue's status updated";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
