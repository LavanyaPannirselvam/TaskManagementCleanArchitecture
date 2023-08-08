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
    public class GetIssuesListDataManager : TaskManagementDataManager, IGetIssuesListDataManager
    {
        public GetIssuesListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetIssues(GetIssuesListRequest request, IUsecaseCallbackBasecase<GetIssuesListResponse> response)
        {
            var assignedIssuesList = DBhandler.AssignedIssuesListOfAProject(request.projectId,request.count,request.skipCount);
            GetIssuesListResponse taskResponse = new GetIssuesListResponse();
            ZResponse<GetIssuesListResponse> zResponse = new ZResponse<GetIssuesListResponse>();
            if (assignedIssuesList.Count > 0)
            {
                zResponse.Response = "";
            }
            else
            {
                zResponse.Response = "Issues not assigned yet :)";
            }
            taskResponse.Data = assignedIssuesList;
            zResponse.Data = taskResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
