
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface ILoginDataManager
    {
        void ValidateUser(LoginRequest request, IUsecaseCallbackBasecase<LoginResponse> response);
    }


    public class LoginRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public string userId;
        public string password;
        
        public LoginRequest(CancellationTokenSource ctsSource, string userId, string password)
        {
            CtsSource = ctsSource;
            this.userId = userId;
            this.password = password;
        }
    }


    public interface IPresenterLoginCallback : IPresenterCallbackBasecase<LoginResponse>
    { }


    public class Login : UsecaseBase<LoginResponse>
    {
        private ILoginDataManager _dataManager;
        private IPresenterLoginCallback _response;
        private LoginRequest _request;
    
        public Login(IPresenterLoginCallback callback, LoginRequest request)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<ILoginDataManager>();
            _response = callback;
            _request = request;
        }

        public override void Action()
        {
            this._dataManager.ValidateUser(_request, new LoginCallback(this));
        }
        

        public class LoginCallback : IUsecaseCallbackBasecase<LoginResponse>
        {
            private Login _login;
            
            public LoginCallback(Login login)
            {
                _login = login;
            }

            public void OnResponseError(BException response)
            {
                _login._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<LoginResponse> response)
            {
                _login._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<LoginResponse> response)
            {
                _login._response.OnSuccessAsync(response);
            }
        }
    }
    
    
    public class LoginResponse : ZResponse<UserCredential> 
    {
        public User currentUser;
        public Role role;
    }
}
