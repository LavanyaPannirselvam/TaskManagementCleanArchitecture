using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Utils;
using User = TaskManagementLibrary.Models.User;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;

namespace TaskManagementLibrary.Data.DBManager
{
    public class CreateUserAccountDataManager : TaskManagementDataManager, ICreateUserAccountDataManager
    {
        public CreateUserAccountDataManager(IDBHandler dbHandler) : base(dbHandler) { }
        string _password;
        public void AddUser(CreateUserRequest request, IUsecaseCallbackBasecase<AddUserResponse> callback)
        {
            AddUserResponse addUserResponse = new AddUserResponse();
            string name = request.name;
            string email = request.email;
            Role userRole = request.role;
            ZResponse<AddUserResponse> zResponse = new ZResponse<AddUserResponse>();

            if (Validator.ValidateEmail(email))
            {
                if (CheckPreviousUsers(email))
                {
                    User user = CreateUser(name, email, userRole);
                    var userId = DBhandler.AddUser(user);
                    UserCredential credential = CreateUserCredentials(email);
                    DBhandler.AddUserCredential(credential);
                    //addUserResponse.newUser = user;
                    credential.Password = _password;
                    addUserResponse.Data= credential;
                    zResponse.Data = addUserResponse;
                    zResponse.Response = "User account created successfully";
                    callback.OnResponseSuccess(zResponse);
                }
                else
                {
                    callback.OnResponseError(new BaseException
                    {
                        exceptionMessage = "User with this email already exists, try again!"
                    });
                }
            }
            else
            {
                zResponse.Response = "Invalid Email, try again!";
                callback.OnResponseFailure(zResponse);
            }
        }   

        private UserCredential CreateUserCredentials(string email)
        {
            _password = Generator.GeneratePassword();
            //var GeneratedPassword = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(_password));
            return new UserCredential
            {
                Email = email,
                Password = _password,
            };
        }
        private User CreateUser(string name, string email, Role role)
        {
            return new User
            {
                Name = name,
                Email = email,
                Role = role
            };
        }
        private bool CheckPreviousUsers(string email)
        {
            return DBhandler.CheckUser(email);
        }
    }
    }

