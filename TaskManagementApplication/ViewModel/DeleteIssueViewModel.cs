using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class DeleteIssueViewModel : DeleteIssueViewModelBase
    {
        DeleteIssue _deleteIssue;
        public override void DeleteIssue(int issueId)
        {
            _deleteIssue = new DeleteIssue(new DeleteIssueRequest(issueId, new CancellationTokenSource()), new PresenterDeleteIssueCallback(this));
            _deleteIssue.Execute();
        }
    }


    public class PresenterDeleteIssueCallback : IPresenterDeleteIssueCallback
    {
        DeleteIssueViewModel _deleteIssue;
        public PresenterDeleteIssueCallback(DeleteIssueViewModel deleteTask)
        {
            _deleteIssue = deleteTask;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deleteIssue.ResponseString = response.Response.ToString();
                _deleteIssue.Notification.NotificationMessage();//notification need to display
            });
        }
    }


    public abstract class DeleteIssueViewModelBase : NotifyPropertyBase
    {
        public abstract void DeleteIssue(int issueId);
        public IDeleteIssueNotification Notification { get; set; }
        private string _responseString = string.Empty;
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

    public interface IDeleteIssueNotification
    {
        void NotificationMessage();
    }
}
