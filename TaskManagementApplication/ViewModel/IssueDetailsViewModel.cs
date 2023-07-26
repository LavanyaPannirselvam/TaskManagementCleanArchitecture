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
using TaskManagementLibrary.Notifications;

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

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetAIssueResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetAIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                //_issueDetailsViewModel.CanRemoveUsersList.Clear();
                //_issueDetailsViewModel.CanAssignUsersList.Clear();
                //PopulateData(response.Data.Data);
                //foreach (var u in response.Data.Data.UsersList)
                //{
                //    if (u.Value == true)
                //        _issueDetailsViewModel.CanRemoveUsersList.Add(u.Key);
                //    else
                //        _issueDetailsViewModel.CanAssignUsersList.Add(u.Key);
                //}
                //if (_issueDetailsViewModel.CanRemoveUsersList.Count == 0)
                //{
                //    _issueDetailsViewModel.TextVisibility = Visibility.Visible;
                //    _issueDetailsViewModel.ListVisibility = Visibility.Collapsed;
                //    _issueDetailsViewModel.ResponseString = response.Response.ToString();
                //}
                //else
                //{
                //    _issueDetailsViewModel.ListVisibility = Visibility.Visible;
                //    _issueDetailsViewModel.TextVisibility = Visibility.Collapsed;
                //}
                _issueDetailsViewModel.SelectedIssue = response.Data.Data;
                _issueDetailsViewModel.AssignedUsersList = response.Data.Data.AssignedUsers;
                if(_issueDetailsViewModel.AssignedUsersList.Count == 0)
                {
                    _issueDetailsViewModel.ListVisibility = Visibility.Collapsed;
                    _issueDetailsViewModel.TextVisibility = Visibility.Visible;
                    _issueDetailsViewModel.ResponseString = "Users not assigned yet:)";
                }
                else
                {
                    _issueDetailsViewModel.ListVisibility = Visibility.Visible;
                    _issueDetailsViewModel.TextVisibility= Visibility.Collapsed;
                }
            });
        }
    }


    public abstract class IssueDetailsViewModelBase : NotifyPropertyBase
    {
        //public ObservableCollection<IssueBO> AIssue = new ObservableCollection<IssueBO>();
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

        public void GetMatchingUsers(string input)
        {
            GetAllMatchingUsers _getAllUsers;
            _getAllUsers = new GetAllMatchingUsers(new GetAllMatchingUsersRequest(input,new CancellationTokenSource()),new PresenterAllMatchingUsersCallback(this));
            _getAllUsers.Execute();
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

        private ObservableCollection<User> _assignedUsersList = new ObservableCollection<User>();
        public ObservableCollection<User> AssignedUsersList
        {
            get { return _assignedUsersList; }
            set
            {
                _assignedUsersList = value;
                OnPropertyChanged(nameof(AssignedUsersList));
            }
        }

        private ObservableCollection<User> _matchingUsers = new ObservableCollection<User>();
        public ObservableCollection<User> MatchingUsers
        {
            get { return _matchingUsers; }
            set
            {
                _matchingUsers = value;
                OnPropertyChanged(nameof(MatchingUsers));
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

        public void OnError(BaseException errorMessage)
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
                UIUpdation.UserAddedUpdate(response.Data.Data);
                //if (_assignIssueToUser.CanAssignUsersList.Count == 0)
                //{
                //    _assignIssueToUser.TextVisibility = Visibility.Visible;
                //    _assignIssueToUser.ListVisibility = Visibility.Collapsed;
                //    _assignIssueToUser.ResponseString = "Users not assigned yet:)";
                //}
                //else
                //{
                //    _assignIssueToUser.ListVisibility = Visibility.Visible;
                //    _assignIssueToUser.TextVisibility = Visibility.Collapsed;
                //}
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

        public void OnError(BaseException errorMessage)
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
                UIUpdation.UserRemovedUpdate(response.Data.Data);
                //if (_removeIssue.CanRemoveUsersList.Count == 0)
                //{
                //    _removeIssue.TextVisibility = Visibility.Visible;
                //    _removeIssue.ListVisibility = Visibility.Collapsed;
                //    _removeIssue.ResponseString = "Users not assigned yet:)";
                //}
                //else
                //{
                //    _removeIssue.ListVisibility = Visibility.Visible;
                //    _removeIssue.TextVisibility = Visibility.Collapsed;
                //}
            });
        }
    }

    public class PresenterAllMatchingUsersCallback : IPresenterGetAllMatchingUsersCallback
    {
        private IssueDetailsViewModelBase _getMatchingUsers;

        public PresenterAllMatchingUsersCallback(IssueDetailsViewModelBase obj)
        {
            _getMatchingUsers = obj;
        }
        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<GetAllMatchingUsersResponse> response)
        {
            throw new NotImplementedException();
        }

        public void OnSuccessAsync(ZResponse<GetAllMatchingUsersResponse> response)
        {
                _getMatchingUsers.MatchingUsers = response.Data.Data;
                //if(_getMatchingUsers.AssignedUsersList.Count>0)
                //{
                //    _getMatchingUsers.ListVisibility = Visibility.Visible;
                //    _getMatchingUsers.TextVisibility = Visibility.Collapsed;
                //}
                //else
                //{
                //    _getMatchingUsers.ListVisibility= Visibility.Collapsed;
                //    _getMatchingUsers.TextVisibility = Visibility.Visible;
                //    _getMatchingUsers.ResponseString = "Users not assigned yet:)";
                //}
        }
    }
}
