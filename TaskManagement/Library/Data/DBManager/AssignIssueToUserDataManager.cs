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
            var userId = request.userId;
            DBhandler.AssignActivity(userId, issueId, ActivityType.ISSUE);
            ZResponse<AssignIssueResponse> zResponse = new ZResponse<AssignIssueResponse>();
            //List<User> users = DbHandler.UsersList();
            List<User> assignedUsers = DBhandler.AssignedUsersList(request.issueId, (int)ActivityType.ISSUE);
            AssignIssueResponse assignIssueResponse = new AssignIssueResponse();
            //foreach (var u in users)
            //{
            //    if (assignedUsers.Contains<User>(u))
            //        assignIssueResponse.users.Add(u);
            //}
            assignIssueResponse.Data = new ObservableCollection<User>(assignedUsers);
            zResponse.Response = "Issue assigned to user successfully";
            zResponse.Data = assignIssueResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
    }

