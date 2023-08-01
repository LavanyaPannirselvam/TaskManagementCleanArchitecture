
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
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
            //string password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.password));
            string password = request.password;
            Role userRole;
            string response;
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> zResponse = new ZResponse<LoginResponse>();
            try
            {
                if (DBhandler.CheckUserCredential(userId, password))
                {
                    if (DBhandler.CheckIfAdmin(userId))
                    {
                        userRole = Role.ADMIN;
                        response = "Welcome Admin";
                    }
                    else if (DBhandler.CheckIfManager(userId))
                    {
                        userRole = Role.MANAGER;
                        response = "Welcome Manager";
                    }
                    else 
                    {
                        userRole = Role.EMPLOYEE;
                        response = "Welcome!";
                    }
                    var user = DBhandler.GetUser(userId);
                    LoggedInUserBO loggedUser = new LoggedInUserBO(user);
                    loggedUser.Role = userRole;
                    loggedUser.WelcomeText = response;
                    loginResponse.Data = loggedUser;
                   // loginResponse.Data.Role = userRole;
                    loginResponse.Response = response;
                    zResponse.Data = loginResponse;
                    zResponse.Response = response;
                    callback.OnResponseSuccess(zResponse);
                }
                else
                {
                    if (DBhandler.CheckUser(userId))
                    {
                        response = "Invalid User, Try Again";
                        zResponse.Response = response;
                        zResponse.Data = null;
                        callback.OnResponseError(new BaseException { exceptionMessage = zResponse.Response });
                    }
                    else
                    {
                        response = "Invalid password";
                        zResponse.Response = response;
                        zResponse.Data = null;
                        callback.OnResponseFailure(zResponse);
                    }
                }
            }
            catch (BaseException ex)
            {
                callback?.OnResponseError(new BaseException(ex));
            }
        }
    }
}
