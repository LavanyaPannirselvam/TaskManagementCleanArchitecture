using Microsoft.Extensions.DependencyInjection;
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
    public interface IGetAllMatchingUsersBODataManager
    {
        void GetAllMatchingUsersBO(GetAllMatchingUsersBORequest request, IUsecaseCallbackBasecase<GetAllMatchingUsersBOResponse> users);
    }

    public class GetAllMatchingUsersBORequest : IRequest
    {
        public string inputText;
        public CancellationTokenSource CtsSource { get; set; }

        public GetAllMatchingUsersBORequest(string inputText, CancellationTokenSource ctsSource)
        {
            this.inputText = inputText;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterGetAllMatchingUsersBOCallback : IPresenterCallbackBasecase<GetAllMatchingUsersBOResponse> { }


    public class GetAllMatchingUsersBO : UsecaseBase<GetAllMatchingUsersBOResponse>
    {
        private IGetAllMatchingUsersBODataManager _dataManager;
        private IPresenterGetAllMatchingUsersBOCallback _response;
        private GetAllMatchingUsersBORequest _request;

        public GetAllMatchingUsersBO(GetAllMatchingUsersBORequest request, IPresenterGetAllMatchingUsersBOCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetAllMatchingUsersBODataManager>();
            _response = response;
            _request = request;
        }

        public override void Action()
        {
            _dataManager.GetAllMatchingUsersBO(_request, new GetAllMatchingUsersBOCallback(this));
        }


        public class GetAllMatchingUsersBOCallback : IUsecaseCallbackBasecase<GetAllMatchingUsersBOResponse>
        {
            private GetAllMatchingUsersBO _getAllMatchingUsers;

            public GetAllMatchingUsersBOCallback(GetAllMatchingUsersBO obj)
            {
                _getAllMatchingUsers = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _getAllMatchingUsers._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetAllMatchingUsersBOResponse> response)
            {
                _getAllMatchingUsers._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAllMatchingUsersBOResponse> response)
            {
                _getAllMatchingUsers._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetAllMatchingUsersBOResponse : ZResponse<ObservableCollection<UserBO>>
    {

    }
}
