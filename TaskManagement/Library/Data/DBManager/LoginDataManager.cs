
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Utils;

namespace TaskManagementLibrary.Data.DBManager
{
    public class LoginDataManager : TaskManagementDataManager, ILoginDataManager
    {
        public LoginDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void ValidateUser(LoginRequest request, IUsecaseCallbackBasecase<LoginResponse> callback)
        {
            string userId = request.userId;
            string password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.password));
            Role userRole;
            string response;
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> zResponse = new ZResponse<LoginResponse>();
            try
            {
                if (DbHandler.CheckUserCredential(userId, password))
                {
                    if (DbHandler.CheckIfAdmin(userId))
                    {
                        userRole = Role.ADMIN;
                        response = "Logged in successfully, Welcome Admin";
                    }
                    else if (DbHandler.CheckIfManager(userId))
                    {
                        userRole = Role.MANAGER;
                        response = "Logged in successfully, Welcome Manager";
                    }
                    else
                    {
                        userRole = Role.EMPLOYEE;
                        response = "Logged in successfully, Welcome!";
                    }
                    loginResponse.currentUser = DbHandler.GetUser(userId);
                    loginResponse.role = userRole;
                    zResponse.Data = loginResponse;
                    zResponse.Response = response;
                    callback.OnResponseSuccess(zResponse);
                }
                else
                {
                    if (DbHandler.CheckUser(userId))
                    {
                        response = "Invalid password";
                        zResponse.Response = response;
                        zResponse.Data = null;
                        callback.OnResponseError(new BException { exceptionMessage = zResponse.Response });
                    }
                    else
                    {
                        response = "Invalid User, Try Again";
                        zResponse.Response = response;
                        zResponse.Data = null;
                        callback.OnResponseFailure(zResponse);
                    }
                }
            }
            catch (NoUserException ex)
            {
                callback?.OnResponseError(new BException(ex));
            }
        }
    }
}
