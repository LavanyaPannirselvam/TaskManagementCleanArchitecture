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
    public class DeleteIssueDataManager : TaskManagementDataManager, IDeleteIssueDataManager
    {
        public DeleteIssueDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void DeleteIssue(DeleteIssueRequest request, IUsecaseCallbackBasecase<DeleteIssueResponse> response)
        {
            var id = request.issueId;
            var data = DBhandler.GetIssue(request.issueId);
            var assignedUsersList = DBhandler.AssignedUsersListOfAIssue(id);
            if (assignedUsersList != null)
            {
                DBhandler.RemoveAllAssignments(assignedUsersList);
            }
            DBhandler.DeleteIssue(id);
            ZResponse<DeleteIssueResponse> zResponse = new ZResponse<DeleteIssueResponse>();
            DeleteIssueResponse deleteIssueResponse = new DeleteIssueResponse();
            deleteIssueResponse.Data = data;
            zResponse.Response = "Issue deleted successfully";
            zResponse.Data = deleteIssueResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
