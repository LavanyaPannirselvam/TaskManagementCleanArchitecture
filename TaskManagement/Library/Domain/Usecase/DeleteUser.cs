using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IDeleteUserDataManager
    {
        void DeleteUser(DeleteUserRequest request , IUsecaseCallbackBasecase<bool> response);
    }

    public class DeleteUserRequest : IRequest
    {
        public string UserEmail;
        public CancellationTokenSource CtsSource { get ; set; }

        public DeleteUserRequest(string userEmail, CancellationTokenSource ctsSource)
        {
            UserEmail = userEmail;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterDeleteUserCallback : IPresenterCallbackBasecase<bool> { }


    public class DeleteUser : UsecaseBase<bool>
    {
        private IDeleteUserDataManager _dataManager;
        private IPresenterDeleteUserCallback _callback;
        private DeleteUserRequest _request;
        
        public DeleteUser(DeleteUserRequest deleteUserRequest,IPresenterDeleteUserCallback presenterDeleteUserCallback)
        {
            _request = deleteUserRequest;
            _callback = presenterDeleteUserCallback;
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IDeleteUserDataManager>();
        }

        public override void Action()
        {
            _dataManager.DeleteUser(_request, new DeleteUserCallback(this));
        }


        public class DeleteUserCallback : IUsecaseCallbackBasecase<bool>
        {
            private DeleteUser _deleteUser;

            public DeleteUserCallback(DeleteUser deleteUser)
            {
                _deleteUser = deleteUser;
            }

            public void OnResponseError(BaseException response)
            {
                _deleteUser._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                _deleteUser._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _deleteUser._callback.OnSuccessAsync(response);
            }
        }
    }


   // public class DeleteUserResponse : ZResponse<bool> { }
}
