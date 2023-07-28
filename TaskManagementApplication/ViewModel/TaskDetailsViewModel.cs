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

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class TaskDetailsViewModel : TaskDetailsViewModelBase
    {
        private GetATask _getATask;
        public override void GetATask(int projectId)
        {
            _getATask = new GetATask(new GetATaskRequest(projectId, new CancellationTokenSource()), new PresenterGetTaskDetails(this));
            _getATask.Execute();
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

        public void RemoveTask(string userEmail, int taskId)
        {
            RemoveTaskFromUser _removeTask;
             _removeTask = new RemoveTaskFromUser(new RemoveTaskRequest(new CancellationTokenSource(), userEmail, taskId), new PresenterRemoveTaskFromUserCallback(this));
            _removeTask.Execute();
        }

        internal void AssignTask(string userEmail, int taskId)
        {
            AssignTaskToUser _assignTaskToUser;
            _assignTaskToUser = new AssignTaskToUser(new AssignTaskRequest(taskId, userEmail, new CancellationTokenSource()), new PresenterAssignTaskCallback(this));
            _assignTaskToUser.Execute();
        }

        public void GetMatchingUsers(string input)
        {
            GetAllMatchingUsers _getAllUsers;
            _getAllUsers = new GetAllMatchingUsers(new GetAllMatchingUsersRequest(input, new CancellationTokenSource()), new PresenterAllMatchingUsersOfTaskCallback(this));
            _getAllUsers.Execute();
        }

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

    public class PresenterAllMatchingUsersOfTaskCallback : IPresenterGetAllMatchingUsersCallback
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

        public void OnFailure(ZResponse<GetAllMatchingUsersResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<GetAllMatchingUsersResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _getMatchingUsers.MatchingUsers = response.Data.Data;
                _getMatchingUsers.updateMatchingUsers.UpdateMatchingUsers();
            });
        }
    }
}

