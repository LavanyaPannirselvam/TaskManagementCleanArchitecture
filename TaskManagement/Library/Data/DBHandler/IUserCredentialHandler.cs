using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IUserCredentialHandler
    {
        bool CheckUserCredential(string userId, string password);
        bool CheckIfAdmin(string userId);
        bool CheckIfManager(string userId);
        //void CreateUserCredential(UserCredential credential);
        void AddUserCredential(UserCredential credential);
        void DeleteUserCredentials(string email);
    }
}
