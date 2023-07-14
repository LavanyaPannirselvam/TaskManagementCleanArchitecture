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
            var newIssue = request.issue;
            var id = DbHandler.AddIssue(newIssue);
            newIssue = DbHandler.GetIssue(id);
            ZResponse<CreateIssueResponse> zResponse = new ZResponse<CreateIssueResponse>();
            CreateIssueResponse createIssueResponse = new CreateIssueResponse();
            createIssueResponse.NewIssue = newIssue;
            zResponse.Response = "Issue added successfully";
            zResponse.Data = createIssueResponse;
            response.OnResponseSuccess(zResponse);
        }
        //event to be created and handled
    }

}
