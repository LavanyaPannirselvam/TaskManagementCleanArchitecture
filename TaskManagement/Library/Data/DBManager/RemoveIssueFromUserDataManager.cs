using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Models.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBManager
{
    public class RemoveIssueFromUserDataManager : TaskManagementDataManager, IRemoveIssueFromUserDataManager
    {
        public RemoveIssueFromUserDataManager(IDBHandler dBHandler) : base(dBHandler) { }

        public void RemoveIssueFromUser(RemoveIssueRequest request, IUsecaseCallbackBasecase<RemoveIssueResponse> callback)
        {
            
            DBhandler.DeassignActivity(request.userEmail, request.issueId, ActivityType.ISSUE);
            ZResponse<RemoveIssueResponse> zResponse = new ZResponse<RemoveIssueResponse>();
            zResponse.Response = "Issue removed from user successfully";
            RemoveIssueResponse removeIssueResponse = new RemoveIssueResponse();
            //List<User> users = DbHandler.UsersList();
            List<UserBO> assignedUsers = DBhandler.AssignedUsersList(request.issueId, (int)ActivityType.ISSUE);
            //foreach (var u in users)
            //{
            //    if (!assignedUsers.Contains<User>(u))
            //        removeIssueResponse.users.Add(u);
            //}
            removeIssueResponse.Data = new System.Collections.ObjectModel.ObservableCollection<UserBO>(assignedUsers);
            zResponse.Data = removeIssueResponse;
            callback.OnResponseSuccess(zResponse);
        }
    }
}
