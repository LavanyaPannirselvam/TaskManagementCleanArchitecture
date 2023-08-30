using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using Windows.UI.Xaml;
using TaskManagementLibrary.Notifications;
using TaskManagementLibrary.Enums;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class TaskDetailsViewModel : TaskDetailsViewModelBase
    {
        public override void GetATask(int projectId)
        {
            GetATask _getATask;
            _getATask = new GetATask(new GetATaskRequest(projectId, new CancellationTokenSource()), new PresenterGetTaskDetails(this));
            _getATask.Execute();
        }

        public override void RemoveTaskFromUser(string userEmail, int taskId)
        {
            RemoveTaskFromUser _removeTask;
            _removeTask = new RemoveTaskFromUser(new RemoveTaskRequest(new CancellationTokenSource(), userEmail, taskId), new PresenterRemoveTaskFromUserCallback(this));
            _removeTask.Execute();
        }

        public override void AssignTaskToUser(string userEmail, int taskId)
        {
            AssignTaskToUser _assignTaskToUser;
            _assignTaskToUser = new AssignTaskToUser(new AssignTaskRequest(taskId, userEmail, new CancellationTokenSource()), new PresenterAssignTaskCallback(this));
            _assignTaskToUser.Execute();
        }

        public override void GetMatchingUsers(string input)
        {
            GetAllMatchingUsersBO _getAllUsers;
            _getAllUsers = new GetAllMatchingUsersBO(new GetAllMatchingUsersBORequest(input, new CancellationTokenSource()), new PresenterAllMatchingUsersOfTaskCallback(this));
            _getAllUsers.Execute();
        }

        public override void ChangePriority(int issueId, PriorityType priorityType)
        {
            ChangeTaskPriority _changePriority;
            _changePriority = new ChangeTaskPriority(new ChangeTaskPriorityRequest(new CancellationTokenSource(), issueId, priorityType), new PresenterChangeTaskPriorityCallback(this));
            _changePriority.Execute();
        }

        public override void ChangeStatus(int issueId, StatusType status)
        {
            ChangeTaskStatus _changeStatus;
            _changeStatus = new ChangeTaskStatus(new ChangeTaskStatusRequest(issueId, status, new CancellationTokenSource()), new PresenterChangeTaskStatusCallback(this));
            _changeStatus.Execute();
        }

        public override void ChangeName(int issueId, string name)
        {
            ChangeTaskName _changeName;
            _changeName = new ChangeTaskName(new ChangeTaskNameRequest(name, issueId, new CancellationTokenSource()), new PresenterChangeNameOfTaskCallback(this));
            _changeName.Execute();
        }

        public override void ChangeDescription(int issueId, string name)
        {
            ChangeTaskDescription _changeDescription;
            _changeDescription = new ChangeTaskDescription(new ChangeTaskDescriptionRequest(name, issueId, new CancellationTokenSource()), new PresenterChangeDescriptionOfTaskCallback(this));
            _changeDescription.Execute();
        }

        public override void ChangeStartDate(int issueId, DateTimeOffset date)
        {
            ChangeStartDateofTask _changeStartDateofTask;
            _changeStartDateofTask = new ChangeStartDateofTask(new ChangeStartDateofTaskRequest(issueId, date, new CancellationTokenSource()), new PresenterChangeStartDateOfTaskCallback(this));
            _changeStartDateofTask.Execute();
        }

        public override void ChangeEndDate(int issueId, DateTimeOffset date)
        {
            ChangeEndDateofTask _changeEndDateofTask;
            _changeEndDateofTask = new ChangeEndDateofTask(new ChangeEndDateofTaskRequest(issueId, date, new CancellationTokenSource()), new PresenterChangeEndDateOfTaskCallback(this));
            _changeEndDateofTask.Execute();
        }

    }


    public class PresenterGetTaskDetails : IPresenterGetATaskListCallback
    {
        private readonly TaskDetailsViewModel _taskDetailsViewModel;

        public PresenterGetTaskDetails(TaskDetailsViewModel viewModel)
        {
            _taskDetailsViewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetATaskResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetATaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _taskDetailsViewModel.SelectedTask = response.Data.Data;
                _taskDetailsViewModel.AssignedUsersList.Clear();
                PopulateData(response.Data.Data.AssignedUsers);
                if (_taskDetailsViewModel.AssignedUsersList.Count == 0)
                {
                    _taskDetailsViewModel.ListVisibility = Visibility.Collapsed;
                    _taskDetailsViewModel.TextVisibility = Visibility.Visible;
                    _taskDetailsViewModel.ResponseString = "Users not assigned yet:)";
                }
                else
                {
                    _taskDetailsViewModel.ListVisibility = Visibility.Visible;
                    _taskDetailsViewModel.TextVisibility = Visibility.Collapsed;
                }
            });
        }

        private void PopulateData(List<UserBO> assignedUsers)
        {
            foreach (var user in assignedUsers)
            {
                _taskDetailsViewModel.AssignedUsersList.Add(user);
            }
        }
    }
    
    public abstract class TaskDetailsViewModelBase : NotifyPropertyBase
    {
        public abstract void GetATask(int taskId);
        public abstract void RemoveTaskFromUser(string userEmail,int taskId);
        public abstract void AssignTaskToUser(string userEmail,int taskId);
        public abstract void GetMatchingUsers(string input);
        public abstract void ChangePriority(int issueId, PriorityType priorityType);
        public abstract void ChangeStatus(int issueId, StatusType statusType);
        public abstract void ChangeName(int issueId, string name);
        public abstract void ChangeDescription(int issueId, string description);
        public abstract void ChangeStartDate(int issueId, DateTimeOffset date);
        public abstract void ChangeEndDate(int issueId, DateTimeOffset date);

        
        public ITaskDetailsNotification taskDetailsNotification { get; set; }
        public IUpdateMatchingUsersOfTask updateMatchingUsers { get; set; }

        private TaskBO _selectedTask;
        public TaskBO SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
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
    }


    public interface ITaskDetailsNotification
    {
        void TaskDetailsNotification();
    }

    public interface IUpdateMatchingUsersOfTask
    {
        void UpdateMatchingUsers();
    }


    public class PresenterRemoveTaskFromUserCallback : IPresenterRemoveTaskFromUserCallback
    {
        private TaskDetailsViewModelBase _removeTask;
        public PresenterRemoveTaskFromUserCallback(TaskDetailsViewModelBase removeTask)
        {
            _removeTask = removeTask;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<RemoveTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<RemoveTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _removeTask.NotificationVisibility = Visibility.Visible;
                _removeTask.ResponseString = response.Response.ToString();
                _removeTask.taskDetailsNotification.TaskDetailsNotification();
                UIUpdation.UserRemovedUpdate(response.Data.Data);
                if (_removeTask.AssignedUsersList.Count == 0)
                {
                    _removeTask.ListVisibility = Visibility.Collapsed;
                    _removeTask.TextVisibility = Visibility.Visible;
                    _removeTask.ResponseString = "Users not assigned yet:)";
                }
                else
                {
                    _removeTask.ListVisibility = Visibility.Visible;
                    _removeTask.TextVisibility = Visibility.Collapsed;
                }
            });
        }
    }


    public class PresenterAssignTaskCallback : IPresenterAssignTaskCallback
    {
        private TaskDetailsViewModelBase _assignTaskToUser;

        public PresenterAssignTaskCallback(TaskDetailsViewModelBase assignTaskToUser)
        {
            _assignTaskToUser = assignTaskToUser;
        }

        public void OnError(BaseException errorMessage)
        {
        }

        public void OnFailure(ZResponse<AssignTaskResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<AssignTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _assignTaskToUser.ResponseString = response.Response.ToString();
                _assignTaskToUser.NotificationVisibility = Visibility.Visible;
                _assignTaskToUser.taskDetailsNotification.TaskDetailsNotification();
                UIUpdation.UserAddedUpdate(response.Data.Data);
                _assignTaskToUser.ListVisibility = Visibility.Visible;
                _assignTaskToUser.TextVisibility = Visibility.Collapsed;
            });
        }
    }


    public class PresenterAllMatchingUsersOfTaskCallback : IPresenterGetAllMatchingUsersBOCallback
    {
        private TaskDetailsViewModelBase _getMatchingUsers;

        public PresenterAllMatchingUsersOfTaskCallback(TaskDetailsViewModelBase obj)
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


    public class PresenterChangeTaskPriorityCallback : IPresenterChangeTaskPriorityCallback
    {
        private TaskDetailsViewModelBase _viewModel;

        public PresenterChangeTaskPriorityCallback(TaskDetailsViewModelBase taskDetailsViewModelBase)
        {
            this._viewModel = taskDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeTaskPriorityResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeTaskPriorityResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                 _viewModel.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }


    public class PresenterChangeTaskStatusCallback : IPresenterChangeTaskStatusCallback
    {
        TaskDetailsViewModelBase _viewModel;
        public PresenterChangeTaskStatusCallback(TaskDetailsViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeTaskStatusResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeTaskStatusResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _viewModel.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }


    public class PresenterChangeNameOfTaskCallback : IPresenterChangeTaskNameCallback
    {
        private TaskDetailsViewModelBase _taskDetailsViewModel;

        public PresenterChangeNameOfTaskCallback(TaskDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._taskDetailsViewModel = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeTaskNameResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeTaskNameResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _taskDetailsViewModel.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }


    public class PresenterChangeDescriptionOfTaskCallback : IPresenterChangeTaskDescriptionCallback
    {
        private TaskDetailsViewModelBase _taskDetailsViewModelBase;

        public PresenterChangeDescriptionOfTaskCallback(TaskDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._taskDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeTaskDescriptionResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeTaskDescriptionResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _taskDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }


    public class PresenterChangeStartDateOfTaskCallback : IPresenterChangeStartDateOfTaskCallback
    {
        private TaskDetailsViewModelBase _taskDetailsViewModelBase;

        public PresenterChangeStartDateOfTaskCallback(TaskDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._taskDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeStartDateofTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeStartDateofTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _taskDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }


    public class PresenterChangeEndDateOfTaskCallback : IPresenterChangeEndDateOfTaskCallback
    {
        private TaskDetailsViewModelBase _issueDetailsViewModelBase;

        public PresenterChangeEndDateOfTaskCallback(TaskDetailsViewModelBase issueDetailsViewModelBase)
        {
            this._issueDetailsViewModelBase = issueDetailsViewModelBase;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<ChangeEndDateofTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<ChangeEndDateofTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _issueDetailsViewModelBase.ResponseString = response.Response;
                UIUpdation.OnTaskUpdate(response.Data.Data);
            });
        }
    }
}

