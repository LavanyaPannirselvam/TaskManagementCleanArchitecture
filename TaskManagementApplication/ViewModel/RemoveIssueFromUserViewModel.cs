using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class RemoveIssueFromUserViewModel : RemoveIssueFromUserViewModelBase
    {
        RemoveIssueFromUser _removeIssue;
        public override void RemoveIssue(int userId, int issueId)
        {
            _removeIssue = new RemoveIssueFromUser(new RemoveIssueRequest(new CancellationTokenSource(), userId, issueId), new PresenterRemoveIssueFromUserCallback(this));
            _removeIssue.Execute();
        }
    }

    public class PresenterRemoveIssueFromUserCallback : IPresenterRemoveIssueFromUserCallback
    {
        private RemoveIssueFromUserViewModel _removeIssue;
        public PresenterRemoveIssueFromUserCallback(RemoveIssueFromUserViewModel removeTask)
        {
            _removeIssue = removeTask;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<RemoveIssueResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<RemoveIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _removeIssue.NotificationVisibility = Visibility.Visible;
                _removeIssue.ResponseString = response.Response.ToString();
                _removeIssue.userRemovedNotifcation.UserRemovedFromIssueUpdate();
            });
        }
    }


    public abstract class RemoveIssueFromUserViewModelBase : NotifyPropertyBase
    {
        public abstract void RemoveIssue(int userId, int issueId);
        public IUserRemovedFromIssueNotification userRemovedNotifcation { get; set; }
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

        private Visibility _notificationVisibility = Visibility.Collapsed;
        public Visibility NotificationVisibility
        {
            get { return _notificationVisibility; }
            set
            {
                _notificationVisibility = value;
                OnPropertyChanged(nameof(NotificationVisibility));
            }
        }
    }


    public interface IUserRemovedFromIssueNotification
    {
        void UserRemovedFromIssueUpdate();
    }
}
