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
    public class GetUsersListDataManager : TaskManagementDataManager, IGetUsersListDataManager
    {
        public GetUsersListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {}

        public void GetUsersList(GetUsersListRequest request, IUsecaseCallbackBasecase<GetUsersListResponse> response)
        {
            var list = DbHandler.AssignedUsersList(request.projectId);
            GetUsersListResponse userResponse = new GetUsersListResponse();
            userResponse.AssignedUserList = list;
            ZResponse<GetUsersListResponse> zResponse = new ZResponse<GetUsersListResponse>();
            zResponse.Response = "";
            zResponse.Data = userResponse;
            response.OnResponseSuccess(zResponse);


        }
    }
}
//to get assigned users of a project should write join query on assignment with the projectid getting the userid and then passing to user table to get the user --> need project id as parameter