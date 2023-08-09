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
using TaskManagementLibrary.Enums;
using Namotion.Reflection;

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
                _issueDetailsViewModel.SelectedIssue = response.Data.Data;
                _issueDetailsViewModel.AssignedUsersList.Clear();
                Populate(response.Data.Data.AssignedUsers);
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

        private void Populate(ObservableCollection<UserBO> assignedUsers)
        {
            foreach(var user in assignedUsers)
            {
                _issueDetailsViewModel.AssignedUsersList.Add(user);
            }
        }
    }


    public abstract class IssueDetailsViewModelBase : NotifyPropertyBase
    {
        public abstract void GetAIssue(int issueId);

        public void RemoveUserFromIssue(string userEmail, int issueId)
        {
            RemoveIssueFromUser _removeIssue;
            _removeIssue = new RemoveIssueFromUser(new RemoveIssueRequest(new CancellationTokenSource(), userEmail, issueId), new PresenterRemoveIssueFromUserCallback(this));
            _removeIssue.Execute();
        }

        public void AssignUserToIssue(string email, int id)
        {
            AssignIssueToUser _assignIssueToUser;
            _assignIssueToUser = new AssignIssueToUser(new AssignIssueRequest(id, email, new CancellationTokenSource()), new PresenterAssignIssueCallback(this));
            _assignIssueToUser.Execute();
        }

        public void GetMatchingUsers(string input)
        {
            GetAllMatchingUsersBO _getAllUsers;
            _getAllUsers = new GetAllMatchingUsersBO(new GetAllMatchingUsersBORequest(input, new CancellationTokenSource()), new PresenterAllMatchingUsersOfIssueCallback(this));
            _getAllUsers.Execute();
        }

        public void ChangePriority(int issueId,PriorityType priorityType)
        {
            ChangeIssuePriority _changePriority;
            _changePriority = new ChangeIssuePriority(new ChangeIssuePriorityRequest(new CancellationTokenSource(), issueId, priorityType), new PresenterChangeIssuePriorityCallback(this));
            _changePriority.Execute();
        }

        public void ChangeStatus(int issueId,StatusType status)
        {
            ChangeIssueStatus _changeIssueStatus;
            _changeIssueStatus = new ChangeIssueStatus(new ChangeIssueStatusRequest(issueId, status, new CancellationTokenSource()),new PresenterChangeIssueStatusCallback(this));
            _changeIssueStatus.Execute();
        }

        public void ChangeName(int issueId,string name)
        {
            ChangeIssueName _changeName;
            _changeName = new ChangeIssueName(new ChangeIssueNameRequest(name, issueId, new CancellationTokenSource()), new PresenterChangeNameOfIssueCallback(this));
            _changeName.Execute();
        }

        public void ChangeDescription(int issueId, string name)
        {
            ChangeIssueDescription _changeDescription;
            _changeDescription = new ChangeIssueDescription(new ChangeIssueDescriptionRequest(name, issueId, new CancellationTokenSource()), new PresenterChangeDescriptionOfIssueCallback(this));
            _changeDescription.Execute();
        }

        public void ChangeStartDate(int issueId,DateTimeOffset date)
        {
            ChangeStartDateofIssue _changeStartDateofIssue;
            _changeStartDateofIssue = new ChangeStartDateofIssue(new ChangeStartDateofIssueRequest(issueId, date, new CancellationTokenSource()), new PresenterChangeStartDateOfIssueCallback(this));
            _changeStartDateofIssue.Execute();
        }

        public void ChangeEndDate(int issueId, DateTimeOffset date)
        {
            ChangeEndDateofIssue _changeEndDateofIssue;
            _changeEndDateofIssue = new ChangeEndDateofIssue(new ChangeEndDateofIssueRequest(issueId, date, new CancellationTokenSource()), new PresenterChangeEndDateOfIssueCallback(this));
            _changeEndDateofIssue.Execute();
        }

        public IIssueDetailsPageNotification issueDetailsPageNotification { get; set; }
        public IUpdateMatchingUsersOfIssue updateMatchingUsers { get; set; }
       
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

        private ObservableCollection<UserBO> _assignedUsersList = new ObservableCollection<UserBO>();
        public ObservableCollection<UserBO> AssignedUsersList
        {
            get { return _assignedUsersList; }
            set
            {
                _assignedUsersList = value;
                OnPropertyChanged(nameof(AssignedUsersList));
            }
        }

        private ObservableCollection<UserBO> _matchingUsers = new ObservableCollection<UserBO>();
        public ObservableCollection<UserBO> MatchingUsers
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


    public interface IUpdateMatchingUsersOfIssue
    {
        void UpdateMatchingUsers();
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
                _assignIssueToUser.ListVisibility = Visibility.Visible;
                _assignIssueToUser.TextVisibility = Visibility.Collapsed;
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
                _removeIssue.AssignedUsersList.Clear();
                UIUpdation.UserRemovedUpdate(response.Data.Data);
                if(_removeIssue.AssignedUsersList.Count == 0)
                {
                    _removeIssue.ListVisibility = Visibility.Collapsed;
                    _removeIssue.TextVisibility = Visibility.Visible;
                    _removeIssue.ResponseString = "Users not assigned yet:)";
                }
                else
                {
                    _removeIssue.ListVisibility = Visibility.Visible;
                    _removeIssue.TextVisibility = Visibility.Collapsed;
                }
            });
        }
    }


    public class PresenterAllMatchingUsersOfIssueCallback : IPresenterGetAllMatchingUsersBOCallback
    {
        private IssueDetailsViewModelBase _getMatchingUsers;

        public PresenterAllMatchingUsersOfIssueCallback(IssueDetailsViewModelBase obj)
        {
            _getMatchingUsers = obj;
        }
        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<GetAllMatchingUsersBOResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<GetAllMatchingUsersBOResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _getMatchingUsers.MatchingUsers = response.Data.Data;
                _getMatchingUsers.updateMatchingUsers.UpdateMatchingUsers();
            });
        }
    }


    public class PresenterChangeIssuePriorityCallback : IPresenterChangeIssuePriorityCallback
    {
        IssueDetailsViewModelBase _viewModel;
        public PresenterChangeIssuePriorityCallback(IssueDetailsViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeIssuePriorityResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeIssuePriorityResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                 _viewModel.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _viewModel.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterChangeIssueStatusCallback : IPresenterChangeIssueStatusCallback
    {
        IssueDetailsViewModelBase _viewModel;
        public PresenterChangeIssueStatusCallback(IssueDetailsViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeIssueStatusResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeIssueStatusResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _viewModel.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _viewModel.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterChangeNameOfIssueCallback : IPresenterChangeIssueNameCallback
    {
        private IssueDetailsViewModelBase _issueDetailsViewModelBase;

        public PresenterChangeNameOfIssueCallback(IssueDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._issueDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeIssueNameResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeIssueNameResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _issueDetailsViewModelBase.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterChangeDescriptionOfIssueCallback : IPresenterChangeIssueDescriptionCallback
    {
        private IssueDetailsViewModelBase _issueDetailsViewModelBase;

        public PresenterChangeDescriptionOfIssueCallback(IssueDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._issueDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeIssueDescriptionResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeIssueDescriptionResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _issueDetailsViewModelBase.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterChangeStartDateOfIssueCallback : IPresenterChangeStartDateOfIssueCallback
    {
        private IssueDetailsViewModelBase _issueDetailsViewModelBase;

        public PresenterChangeStartDateOfIssueCallback(IssueDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._issueDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeStartDateofIssueResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeStartDateofIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _issueDetailsViewModelBase.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }


    public class PresenterChangeEndDateOfIssueCallback : IPresenterChangeEndDateOfIssueCallback
    {
        private IssueDetailsViewModelBase _issueDetailsViewModelBase;

        public PresenterChangeEndDateOfIssueCallback(IssueDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._issueDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeEndDateofIssueResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeEndDateofIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnIssueUpdate(response.Data.Data);
                _issueDetailsViewModelBase.issueDetailsPageNotification.IssueDetailsPageNotification();
            });
        }
    }
}
