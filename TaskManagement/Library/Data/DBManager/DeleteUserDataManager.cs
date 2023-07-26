using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;

namespace TaskManagementLibrary.Data.DBManager
{
    internal class DeleteUserDataManager : TaskManagementDataManager, IDeleteUserDataManager
    {
        public DeleteUserDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void DeleteUser(DeleteUserRequest request, IUsecaseCallbackBasecase<bool> response)
        {
            var email = request.UserEmail;
            var assignmentsList = DBhandler.AssignmentsList(DBhandler.GetUser(email).UserId);
            if (assignmentsList != null)
            {
                DBhandler.RemoveAllAssignments(assignmentsList);
            }
            DBhandler.DeleteUserCredentials(email);
            DBhandler.DeleteUser(email);
           // DeleteUserResponse deleteUser = new DeleteUserResponse();
            ZResponse<bool> zResponse = new ZResponse<bool>();
            zResponse.Data = true;
            zResponse.Response = "User deleted Successfully";
            response.OnResponseSuccess(zResponse);
        }
    }
}
