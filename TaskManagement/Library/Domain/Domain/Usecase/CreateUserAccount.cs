using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface ICreateUserAccountDataManager
    {
        void AddUser(CreateUserRequest request, IUsecaseCallbackBasecase<AddUserResponse> response);
    }


    public class CreateUserRequest : IRequest
    {
        public string name;
        public string email;
        public Role role;
        public CancellationTokenSource CtsSource { get; set; }

        public CreateUserRequest(string name, string email, Role role, CancellationTokenSource ctsSource)
        {
            this.name = name;
            this.email = email;
            this.role = role;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterCreateUserCallback : IPresenterCallbackBasecase<AddUserResponse> { }


    public class CreateUserAccount : UsecaseBase<AddUserResponse>
    {
        private ICreateUserAccountDataManager _dataManager;
        private IPresenterCreateUserCallback _response;
        private CreateUserRequest _request;

        public CreateUserAccount(IPresenterCreateUserCallback response, CreateUserRequest request)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<ICreateUserAccountDataManager>();
            _response = response;
            _request = request;
        }

        public override void Action()
        {
            this._dataManager.AddUser(_request, new CreateUserCallback(this));
        }


        public class CreateUserCallback : IUsecaseCallbackBasecase<AddUserResponse>
        {
            private CreateUserAccount _userAccountCreation;
            public CreateUserCallback(CreateUserAccount userAccountCreation)
            {
                _userAccountCreation = userAccountCreation;
            }

            public void OnResponseError(BException response)
            {
               _userAccountCreation._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<AddUserResponse> response)
            {
                _userAccountCreation._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<AddUserResponse> response)
            {
                _userAccountCreation._response.OnSuccessAsync(response);
            }
        }
    }


    public class AddUserResponse : ZResponse<User>
    {
        public User newUser;
        public UserCredential Credential;
    }
}
