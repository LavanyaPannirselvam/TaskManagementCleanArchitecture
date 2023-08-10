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
            var issue = DBhandler.GetIssue(request.issueId);
            issue.Status = status;
            DBhandler.UpdateIssue(issue);
            ZResponse<ChangeIssueStatusResponse> zResponse = new ZResponse<ChangeIssueStatusResponse>();
            ChangeIssueStatusResponse response = new ChangeIssueStatusResponse();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Response = "";
            zResponse.Data = response;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
