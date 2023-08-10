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
    public class ChangeIssueNameDataManager : TaskManagementDataManager, IChangeIssueNameDataManager
    {
        public ChangeIssueNameDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ChangeName(ChangeIssueNameRequest request, IUsecaseCallbackBasecase<ChangeIssueNameResponse> callback)
        {
            var issue = DBhandler.GetIssue(request.issueId);
            issue.Name = request.name;
            DBhandler.UpdateIssue(issue);
            ChangeIssueNameResponse response = new ChangeIssueNameResponse();
            ZResponse<ChangeIssueNameResponse> zResponse = new ZResponse<ChangeIssueNameResponse>();
            response.Data = DBhandler.GetIssue(request.issueId);
            zResponse.Data = response;
            zResponse.Response = "";
            callback.OnResponseSuccess(zResponse);
        }
    }
}
