using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetUsersListDataManager
    {
        void GetUsersList(GetUsersListRequest request,IUsecaseCallbackBasecase<GetUsersListResponse> response);
    }


    public class GetUsersListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public GetUsersListRequest(CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterGetUsersListCallback : IPresenterCallbackBasecase<GetUsersListResponse> { }


    public class GetUsersList :UsecaseBase<GetUsersListResponse>
    {
        private IGetUsersListDataManager _dataManager;
        private GetUsersListRequest _request;
        private IPresenterGetUsersListCallback _response;
        
        public GetUsersList(GetUsersListRequest request, IPresenterGetUsersListCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetUsersListDataManager>();
            _request = request;
            _response = callback;
        }

        public override void Action()
        {
            _dataManager.GetUsersList(_request, new GetUsersListCallback(this));
        }


        public class GetUsersListCallback : IUsecaseCallbackBasecase<GetUsersListResponse>
        {
            private GetUsersList _usersList;
            
            public GetUsersListCallback(GetUsersList list)
            {
                _usersList = list;
            }
            
            public void OnResponseError(BaseException response)
            {
                _usersList._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetUsersListResponse> response)
            {
                _usersList._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetUsersListResponse> response)
            {
                _usersList._response.OnSuccessAsync(response);
            }           
        }
    }


    public class GetUsersListResponse : ZResponse<List<User>>
    {
        //public List<User> UsersList;
    }
}
