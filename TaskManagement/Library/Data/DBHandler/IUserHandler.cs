using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IUserHandler
    {
        int AddUser(User user);
        void DeleteUser(string email);
        User GetUser(int userId);
        User GetUser(string email);
        bool CheckUser(string email);
        List<User> UsersList();
        List<UserBO> AssignedUsersList(int activityId,int activityType);
        List<Assignment> AssignmentsList(string userEmail);
        List<UserBO> MatchingUsers(string input);
    }
}
