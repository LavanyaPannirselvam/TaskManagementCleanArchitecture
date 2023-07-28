using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Data.DBManager
{
    public class AssignIssueToUserDataManager : TaskManagementDataManager, IAssignIssueToUserDataManager
    {
        public AssignIssueToUserDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void AssignIssueToUser(AssignIssueRequest request, IUsecaseCallbackBasecase<AssignIssueResponse> callback)
        {
            var issueId = request.issueId;
            var userEmail = request.userEmail;
            DBhandler.AssignActivity(userEmail, issueId, ActivityType.ISSUE);
            ZResponse<AssignIssueResponse> zResponse = new ZResponse<AssignIssueResponse>();
            //List<User> users = DbHandler.UsersList();
            List<UserBO> assignedUsers = DBhandler.AssignedUsersList(issueId,(int)ActivityType.ISSUE);
            AssignIssueResponse assignIssueResponse = new AssignIssueResponse();
            //foreach (var u in users)
            //{
            //    if (assignedUsers.Contains<User>(u))
            //        assignIssueResponse.users.Add(u);
            //}
            assignIssueResponse.Data = new ObservableCollection<UserBO>(assignedUsers);
            zResponse.Response = "Issue assigned to user successfully";
            zResponse.Data = assignIssueResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}

