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
    public class CreateIssueDataManager : TaskManagementDataManager, ICreateIssueDataManager
    {
        public CreateIssueDataManager(IDBHandler dbHandler) : base(dbHandler) { }

        public void CreateIssue(CreateIssueRequest request, IUsecaseCallbackBasecase<CreateIssueResponse> response)
        {
            //var newIssue = request.issue;
            var id = DBhandler.AddIssue(request.issue);
            ZResponse<CreateIssueResponse> zResponse = new ZResponse<CreateIssueResponse>();
            CreateIssueResponse createIssueResponse = new CreateIssueResponse();
            createIssueResponse.Data = DBhandler.GetIssue(id);
            zResponse.Response = "Issue added successfully";
            zResponse.Data = createIssueResponse;
            response.OnResponseSuccess(zResponse);
        }
        
    }

}
