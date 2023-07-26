using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetAllMatchingUsersDataManager
    {
        void GetAllMatchingUsers(GetAllMatchingUsersRequest request, IUsecaseCallbackBasecase<GetAllMatchingUsersResponse> users);
    }

    public class GetAllMatchingUsersRequest : IRequest
    {
        public string inputText;
        public CancellationTokenSource CtsSource { get ; set; }

        public GetAllMatchingUsersRequest(string inputText, CancellationTokenSource ctsSource)
        {
            this.inputText = inputText;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterGetAllMatchingUsersCallback : IPresenterCallbackBasecase<GetAllMatchingUsersResponse> { }


    public class GetAllMatchingUsers : UsecaseBase<GetAllMatchingUsersResponse>
    {
        private IGetAllMatchingUsersDataManager _dataManager;
        private IPresenterGetAllMatchingUsersCallback _response;
        private GetAllMatchingUsersRequest _request;

        public GetAllMatchingUsers(GetAllMatchingUsersRequest request,IPresenterGetAllMatchingUsersCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetAllMatchingUsersDataManager>();
            _response = response;
            _request = request;
        }

        public override void Action()
        {
            _dataManager.GetAllMatchingUsers(_request, new GetAllMatchingUsersCallback(this));
        }


        public class GetAllMatchingUsersCallback : IUsecaseCallbackBasecase<GetAllMatchingUsersResponse>
        {
            private GetAllMatchingUsers _getAllMatchingUsers;

            public GetAllMatchingUsersCallback(GetAllMatchingUsers obj)
            {
                _getAllMatchingUsers = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _getAllMatchingUsers._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetAllMatchingUsersResponse> response)
            {
                _getAllMatchingUsers._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAllMatchingUsersResponse> response)
            {
                _getAllMatchingUsers._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetAllMatchingUsersResponse : ZResponse<ObservableCollection<User>>
    {

    }
}
