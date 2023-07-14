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

        public void DeleteIssue(DeleteIssueRequest request, IUsecaseCallbackBasecase<bool> response)
        {
            var id = request.issueId;
            DbHandler.DeleteIssue(id);
            ZResponse<bool> zResponse = new ZResponse<bool>();
            zResponse.Response = "Issue deleted successfully";
            zResponse.Data = true;
            response.OnResponseSuccess(zResponse);
        }
    }
}
