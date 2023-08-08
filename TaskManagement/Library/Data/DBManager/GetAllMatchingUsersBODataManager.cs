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
    internal class GetAllMatchingUsersBODataManager : TaskManagementDataManager, IGetAllMatchingUsersBODataManager
    {
        public GetAllMatchingUsersBODataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetAllMatchingUsersBO(GetAllMatchingUsersBORequest request, IUsecaseCallbackBasecase<GetAllMatchingUsersBOResponse> callback)
        {
            var usersList = DBhandler.MatchingUsersBO(request.inputText);
            GetAllMatchingUsersBOResponse usersResponse = new GetAllMatchingUsersBOResponse();
            usersResponse.Data = new ObservableCollection<UserBO>(usersList);
            ZResponse<GetAllMatchingUsersBOResponse> zResponse = new ZResponse<GetAllMatchingUsersBOResponse>();
            zResponse.Data = usersResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
