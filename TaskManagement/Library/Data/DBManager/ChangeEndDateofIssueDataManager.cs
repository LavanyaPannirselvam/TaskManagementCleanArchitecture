using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;

namespace TaskManagementLibrary.Data.DBManager
{
    public class ChangeEndDateofIssueDataManager : TaskManagementDataManager, IChangeEndDateofIssueDataManager
    {
        public ChangeEndDateofIssueDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeEndDate(ChangeEndDateofIssueRequest request, IUsecaseCallbackBasecase<ChangeEndDateofIssueResponse> callback)
        {
            var issue = DBhandler.GetIssue(request.issueId);
            issue.EndDate = request.date;
            DBhandler.UpdateIssue(issue);
            ChangeEndDateofIssueResponse response = new ChangeEndDateofIssueResponse();
            ZResponse<ChangeEndDateofIssueResponse> zResponse = new ZResponse<ChangeEndDateofIssueResponse>();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
