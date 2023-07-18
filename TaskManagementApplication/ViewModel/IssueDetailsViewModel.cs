using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Models;
using TaskManagementLibrary;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class IssueDetailsViewModel : IssueDetailsViewModelBase
    {
        private GetAIssue _getAIssue;
        public override void GetAIssue(int issueId)
        {
            _getAIssue = new GetAIssue(new GetAIssueRequest(issueId, new CancellationTokenSource()), new PresenterGetAIssue(this));
            _getAIssue.Execute();
        }
    }


    public class PresenterGetAIssue : IPresenterGetAIssueListCallback
    {
        private readonly IssueDetailsViewModel _issueDetailsViewModel;

        public PresenterGetAIssue(IssueDetailsViewModel viewModel)
        {
            _issueDetailsViewModel = viewModel;
        }

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetAIssueResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetAIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModel.CanRemoveUsersList.Clear();
                _issueDetailsViewModel.CanAssignUsersList.Clear();
                PopulateData(response.Data.issue);
                foreach (var u in response.Data.issue.UsersList)
                {
                    if (u.Value == true)
                        _issueDetailsViewModel.CanRemoveUsersList.Add(u.Key);
                    else
                        _issueDetailsViewModel.CanAssignUsersList.Add(u.Key);
                }
                if (_issueDetailsViewModel.CanRemoveUsersList.Count == 0)
                {
                    _issueDetailsViewModel.TextVisibility = Visibility.Visible;
                    _issueDetailsViewModel.ListVisibility = Visibility.Collapsed;
                    _issueDetailsViewModel.ResponseString = response.Response.ToString();
                }
                else
                {
                    _issueDetailsViewModel.ListVisibility = Visibility.Visible;
                    _issueDetailsViewModel.TextVisibility = Visibility.Collapsed;
                }
            });
        }

        private void PopulateData(IssueBO data)
        {
            _issueDetailsViewModel.SelectedIssue = data;
            _issueDetailsViewModel.AIssue.Add(data);
        }
    }


    public abstract class IssueDetailsViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<IssueBO> AIssue = new ObservableCollection<IssueBO>();
        public abstract void GetAIssue(int issueId);

        public void RemoveUserFromIssue(int userId, int issueId)
        {
            RemoveIssueFromUser _removeIssue;
            _removeIssue = new RemoveIssueFromUser(new RemoveIssueRequest(new CancellationTokenSource(), userId, issueId), new PresenterRemoveIssueFromUserCallback(this));
            _removeIssue.Execute();

        }

        public void AssignUserToIssue(int userId, int id)
        {
            AssignIssueToUser _assignIssueToUser;
            _assignIssueToUser = new AssignIssueToUser(new AssignIssueRequest(id, userId, new CancellationTokenSource()), new PresenterAssignIssueCallback(this));
            _assignIssueToUser.Execute();
        
        }

        public IIssueDetailsPageNotification issueDetailsPageNotification { get; set; }
        
        private IssueBO _selectedIssue;
        public IssueBO SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                OnPropertyChanged(nameof(SelectedIssue));
            }
        }

        private Visibility _textVisibility = Visibility.Collapsed;
        public Visibility TextVisibility
        {
            get { return _textVisibility; }
            set
            {
                _textVisibility = value;
                OnPropertyChanged(nameof(TextVisibility));
            }
        }

        private Visibility _listVisibility = Visibility.Collapsed;
        public Visibility ListVisibility
        {
            get { return _listVisibility; }
            set
            {
                _listVisibility = value;
                OnPropertyChanged(nameof(ListVisibility));
            }
        }

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

       private ObservableCollection<User> _canAssignUsersList = new ObservableCollection<User>();
        public ObservableCollection<User> CanAssignUsersList
        {
            get { return _canAssignUsersList; }
            set
            {
                _canAssignUsersList = value;
                OnPropertyChanged(nameof(CanAssignUsersList));
            }
        }

        private ObservableCollection<User> _canRemoveUsersList = new ObservableCollection<User>();
        public ObservableCollection<User> CanRemoveUsersList
        {
            get { return _canRemoveUsersList; }
            set
            {
                _canRemoveUsersList = value;
                OnPropertyChanged(nameof(CanRemoveUsersList));
            }
        }

        private Visibility _notificationVisibility;
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


    public interface IIssueDetailsPageNotification
    {
        void IssueDetailsPageNotification();
    }


    public class PresenterAssignIssueCallback : IPresenterAssignIssueCallback
    {
        private IssueDetailsViewModelBase _assignIssueToUser;

        public PresenterAssignIssueCallback(IssueDetailsViewModelBase assignTaskToUser)
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
                _assignIssueToUser.NotificationVisibility = Visibility.Visible;
                _assignIssueToUser.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterRemoveIssueFromUserCallback : IPresenterRemoveIssueFromUserCallback
    {
        private IssueDetailsViewModelBase _removeIssue;
        public PresenterRemoveIssueFromUserCallback(IssueDetailsViewModelBase removeTask)
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
                _removeIssue.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


}
