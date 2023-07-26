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
                //_taskDetailsViewModel.CanRemoveUsersList.Clear();
                //_taskDetailsViewModel.CanAssignUsersList.Clear();
                PopulateData(response.Data.Data);
                //foreach (var u in response.Data.Data.AssignedUsers)
                //{
                //    if (u.Value == true)
                //        _taskDetailsViewModel.CanRemoveUsersList.Add(u.Key);
                //    else
                //        _taskDetailsViewModel.CanAssignUsersList.Add(u.Key);
                //}
                //if (_taskDetailsViewModel.CanRemoveUsersList.Count == 0)
                //{
                //    _taskDetailsViewModel.TextVisibility = Visibility.Visible;
                //    _taskDetailsViewModel.ListVisibility = Visibility.Collapsed;
                //    _taskDetailsViewModel.ResponseString = response.Response.ToString();
                  
                //}
                //else
                //{
                //    _taskDetailsViewModel.ListVisibility = Visibility.Visible;
                //    _taskDetailsViewModel.TextVisibility = Visibility.Collapsed;
                //}
            });
        }

        private void PopulateData(TaskBO data)
        {
            _taskDetailsViewModel.SelectedTask = data;
            _taskDetailsViewModel.ATask.Add(data);
        }
    }
    public abstract class TaskDetailsViewModelBase : NotifyPropertyBase
    {
        public abstract void GetATask(int taskId);

        public void RemoveTask(int userId, int taskId)
        {
            RemoveTaskFromUser _removeTask;
             _removeTask = new RemoveTaskFromUser(new RemoveTaskRequest(new CancellationTokenSource(), userId, taskId), new PresenterRemoveTaskFromUserCallback(this));
            _removeTask.Execute();
        }

        internal void AssignTask(int userId, int taskId)
        {
            AssignTaskToUser _assignTaskToUser;
            _assignTaskToUser = new AssignTaskToUser(new AssignTaskRequest(taskId, userId, new CancellationTokenSource()), new PresenterAssignTaskCallback(this));
            _assignTaskToUser.Execute();
        }

        public ObservableCollection<TaskBO> ATask = new ObservableCollection<TaskBO>();
        public ITaskDetailsNotification taskDetailsNotification { get; set; }
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

        
        //private ObservableCollection<User> _canAssignUsersList = new ObservableCollection<User>();
        //public ObservableCollection<User> CanAssignUsersList
        //{
        //    get { return _canAssignUsersList; }
        //    set
        //    {
        //        _canAssignUsersList = value;
        //        OnPropertyChanged(nameof(CanAssignUsersList));
        //    }
        //}

        //private ObservableCollection<User> _canRemoveUsersList = new ObservableCollection<User>();
        //public ObservableCollection<User> CanRemoveUsersList
        //{
        //    get { return _canRemoveUsersList; }
        //    set
        //    {
        //        _canRemoveUsersList = value;
        //        OnPropertyChanged(nameof(CanRemoveUsersList));
        //    }
        //}
    }


    public interface ITaskDetailsNotification
    {
        void TaskDetailsNotification();
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
                //if (_removeTask.CanRemoveUsersList.Count == 0)
                //{
                //    _removeTask.TextVisibility = Visibility.Visible;
                //    _removeTask.ListVisibility = Visibility.Collapsed;
                //    _removeTask.ResponseString = "Users not assigned yet:)";
                //}
                //else
                //{
                //    _removeTask.ListVisibility = Visibility.Visible;
                //    _removeTask.TextVisibility = Visibility.Collapsed;
                //}
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
                //if (_assignTaskToUser.CanAssignUsersList.Count == 0)
                //{
                //    _assignTaskToUser.TextVisibility = Visibility.Visible;
                //    _assignTaskToUser.ListVisibility = Visibility.Collapsed;
                //    _assignTaskToUser.ResponseString = "Users not assigned yet:)";
                //}
                //else
                //{
                //    _assignTaskToUser.ListVisibility = Visibility.Visible;
                //    _assignTaskToUser.TextVisibility = Visibility.Collapsed;
                //}
            });
        }
    }
}

