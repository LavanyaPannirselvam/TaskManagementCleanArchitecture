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

        public void AddUser(CreateUserRequest request, IUsecaseCallbackBasecase<AddUserResponse> callback)
        {
            AddUserResponse addUserResponse = new AddUserResponse();
            string name = request.name;
            string email = request.email;
            Role userRole = request.role;
            if (Validator.ValidateEmail(email))
            {
                if (CheckPreviousUsers(email))
                {
                    User user = CreateUser(name, email, userRole);
                    DbHandler.AddUser(user);
                    UserCredential credential = CreateUserCredentials(email);
                    DbHandler.AddUserCredential(credential);
                    addUserResponse.newUser = user;
                    addUserResponse.Credential= credential;
                    ZResponse<AddUserResponse> zResponse = new ZResponse<AddUserResponse>();
                    zResponse.Data = addUserResponse;
                    zResponse.Response = "User account created successfully";
                    callback.OnResponseSuccess(zResponse);
                }
                else
                {
                    callback.OnResponseError(new BException
                    {
                        exceptionMessage = "User with this email already exists, try again!"
                    });
                }
            }
            else
            {
                callback.OnResponseError(new BException
                {
                    exceptionMessage = "Invalid Email, try again!"
                });
            }
        }   

        private UserCredential CreateUserCredentials(string email)
        {
            string _password = Generator.GeneratePassword();
            var GeneratedPassword = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(_password));
            return new UserCredential
            {
                Email = email,
                Password = GeneratedPassword,
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
            return DbHandler.CheckUser(email);
        }
    }
    }

