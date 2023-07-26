using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Models.Enums;
using System.Collections.ObjectModel;

namespace TaskManagementLibrary.Data.DBManager
{
    public class GetAIssueDataManager : TaskManagementDataManager, IGetAIssueDataManager
    {
        public GetAIssueDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetAIssue(GetAIssueRequest request, IUsecaseCallbackBasecase<GetAIssueResponse> response)
        {
            var item = DBhandler.GetIssue(request.issueId);
            IssueBO issueBO = new IssueBO(item);
            //issueBO.Issue = item;
            List<User> list = DBhandler.AssignedUsersList(request.issueId, (int)ActivityType.ISSUE);
           // List<User> userList = DbHandler.UsersList();
            ZResponse<GetAIssueResponse> zResponse = new ZResponse<GetAIssueResponse>();
            GetAIssueResponse issueResponse = new GetAIssueResponse();
            //if (list.Count > 0)
            //{
            //    //taskBO.AssignedUsers = list;
            //    foreach (var user in userList)
            //    {
            //        if (list.Contains<User>(user))
            //            issueBO.UsersList.Add(user, true);
            //        else
            //            issueBO.UsersList.Add(user, false);
            //    }
            //    zResponse.Response = "";
            //}
            //else
            //{
            //    zResponse.Response = "Users not assigned yet :)";
            //    foreach (var user in userList)
            //        issueBO.UsersList.Add(user, false);
            //}
            if(list.Count == 0)
            {
                zResponse.Response = "Users not assigned yet :)";
            }
            issueBO.AssignedUsers = new ObservableCollection<User>(list);
            issueResponse.Data = issueBO;
            zResponse.Data = issueResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
