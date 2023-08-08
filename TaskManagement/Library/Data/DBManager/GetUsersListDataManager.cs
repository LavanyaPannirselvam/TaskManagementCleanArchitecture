using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class GetUsersListDataManager : TaskManagementDataManager, IGetUsersListDataManager
    {
        public GetUsersListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {}
        public void GetUsersList(GetUsersListRequest request, IUsecaseCallbackBasecase<GetUsersListResponse> response)
        {
            var entireList = DBhandler.UsersList(request.toFetchCount,request.skipCount);
            GetUsersListResponse userResponse = new GetUsersListResponse();
            //var list = (from u in entireList
            //            select u).Skip(request.pageSize * request.itemsPerPage).Take(request.itemsPerPage);
            userResponse.Data = entireList;
            ZResponse<GetUsersListResponse> zResponse = new ZResponse<GetUsersListResponse>();
            zResponse.Response = "";
            zResponse.Data = userResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
