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

        public void DeleteUser(DeleteUserRequest request, IUsecaseCallbackBasecase<DeleteUserResponse> response)
        {
            var email = request.UserEmail;
            var user = DBhandler.GetUser(email);
            var assignmentsList = DBhandler.AssignmentsList(user.Email);
            if (assignmentsList != null)
            {
                DBhandler.RemoveAllAssignments(assignmentsList);
            }
            DBhandler.DeleteUserCredentials(email);
            DeleteUserResponse userResponse = new DeleteUserResponse();
            userResponse.Data = user;
            DBhandler.DeleteUser(email);
           // DeleteUserResponse deleteUser = new DeleteUserResponse();
            ZResponse<DeleteUserResponse> zResponse = new ZResponse<DeleteUserResponse>();
            zResponse.Data = userResponse;
            zResponse.Response = "User deleted Successfully";
            response.OnResponseSuccess(zResponse);
        }
    }
}
