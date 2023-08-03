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
            PriorityType priorityType = request.priority;
            int issueId = request.issueId;
            var issue = DBhandler.GetIssue(issueId);
            issue.Priority = priorityType;
            DBhandler.UpdateIssue(issue);
            ZResponse<ChangeIssuePriorityResponse> zResponse = new ZResponse<ChangeIssuePriorityResponse>();
            ChangeIssuePriorityResponse response = new ChangeIssuePriorityResponse();
            response.Data = priorityType;
            zResponse.Response = "Issue's priority is updated successfully";
            zResponse.Data = null;
            callback.OnResponseSuccess(zResponse);

        }
    }
}
