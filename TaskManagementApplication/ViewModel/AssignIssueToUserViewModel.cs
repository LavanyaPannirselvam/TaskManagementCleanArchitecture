using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class AssignIssueToUserViewModel : AssignIssueToUserViewModelBase
    {
        AssignIssueToUser _assignIssueToUser;
        public override void AssignUserToIssue(int userId, int issueId)
        {
            _assignIssueToUser = new AssignIssueToUser(new AssignIssueRequest(issueId, userId, new CancellationTokenSource()), new PresenterAssignIssueCallback(this));
            _assignIssueToUser.Execute();
        }
    }

    public class PresenterAssignIssueCallback : IPresenterAssignIssueCallback
    {
        private AssignIssueToUserViewModel _assignIssueToUser;

        public PresenterAssignIssueCallback(AssignIssueToUserViewModel assignTaskToUser)
        {
            _assignIssueToUser = assignTaskToUser;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<AssignIssueResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<AssignIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _assignIssueToUser.ResponseString = response.Response.ToString();
                _assignIssueToUser.assignUser.UpdateIssueAssignment();
            });
        }

    }

    public abstract class AssignIssueToUserViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public IUserIssueAssignedNotification assignUser { get; set; }
        public abstract void AssignUserToIssue(int userId, int issueId);

        private string _responseString;
        public string ResponseString
        {
            get { return _responseString; }
            set
            {
                _responseString = value;
                OnPropertyChanged(nameof(ResponseString));
            }
        }
    }


    public interface IUserIssueAssignedNotification
    {
        void UpdateIssueAssignment();
    }

}
