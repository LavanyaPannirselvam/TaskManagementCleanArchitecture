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
    public class ChangeIssueDescriptionDataManager : TaskManagementDataManager, IChangeIssueDescriptionDataManager
    {
        public ChangeIssueDescriptionDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeDescription(ChangeIssueDescriptionRequest request, IUsecaseCallbackBasecase<ChangeIssueDescriptionResponse> callback)
        {
            var issue = DBhandler.GetIssue(request.issueId);
            issue.Desc = request.description;
            DBhandler.UpdateIssue(issue);
            ChangeIssueDescriptionResponse response = new ChangeIssueDescriptionResponse();
            ZResponse<ChangeIssueDescriptionResponse> zResponse = new ZResponse<ChangeIssueDescriptionResponse>();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
