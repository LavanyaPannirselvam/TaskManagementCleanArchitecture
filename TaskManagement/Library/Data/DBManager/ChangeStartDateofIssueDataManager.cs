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
    public class ChangeStartDateofIssueDataManager : TaskManagementDataManager, IChangeStartDateofIssueDataManager
    {
        public ChangeStartDateofIssueDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeStartDate(ChangeStartDateofIssueRequest request, IUsecaseCallbackBasecase<ChangeStartDateofIssueResponse> callback)
        {
            var issue = DBhandler.GetIssue(request.issueId);
            issue.StartDate = request.date;
            DBhandler.UpdateIssue(issue);
            ChangeStartDateofIssueResponse response = new ChangeStartDateofIssueResponse();
            ZResponse<ChangeStartDateofIssueResponse> zResponse = new ZResponse<ChangeStartDateofIssueResponse>();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
