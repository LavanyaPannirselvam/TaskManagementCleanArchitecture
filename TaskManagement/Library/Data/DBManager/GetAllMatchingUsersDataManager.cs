using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBManager
{
    internal class GetAllMatchingUsersDataManager : TaskManagementDataManager, IGetAllMatchingUsersDataManager
    {
        public GetAllMatchingUsersDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetAllMatchingUsers(GetAllMatchingUsersRequest request, IUsecaseCallbackBasecase<GetAllMatchingUsersResponse> callback)
        {
            var usersList = DBhandler.MatchingUsers(request.inputText);
            GetAllMatchingUsersResponse usersResponse = new GetAllMatchingUsersResponse();
            usersResponse.Data = new ObservableCollection<User>(usersList);
            ZResponse<GetAllMatchingUsersResponse> zResponse = new ZResponse<GetAllMatchingUsersResponse>();
            zResponse.Data = usersResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
