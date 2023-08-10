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
    public class ChangeIssuePriorityDataManager : TaskManagementDataManager, IChangeIssuePriorityDataManager
    {
        public ChangeIssuePriorityDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void ChangePriorityOfIssue(ChangeIssuePriorityRequest request, IUsecaseCallbackBasecase<ChangeIssuePriorityResponse> callback)
        {
            var issue = DBhandler.GetIssue(request.issueId);
            issue.Priority = request.priority;
            DBhandler.UpdateIssue(issue);
            ZResponse<ChangeIssuePriorityResponse> zResponse = new ZResponse<ChangeIssuePriorityResponse>();
            ChangeIssuePriorityResponse response = new ChangeIssuePriorityResponse();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Response = "";
            zResponse.Data = response;
            callback.OnResponseSuccess(zResponse);

        }
    }
}
