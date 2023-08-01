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
using static TaskManagementLibrary.Domain.Usecase.DeleteTask;
using TaskManagementLibrary.Enums;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class TaskPageViewModel : TasksPageViewModelBase
    {
        private GetTasksList _getTasksList;

        public override void GetTasks(int projectId)
        {
            _getTasksList = new GetTasksList(new GetTasksListRequest(projectId, new CancellationTokenSource()), new PresenterGetTasksList(this));
            _getTasksList.Execute();
        }
    }


    public class PresenterGetTasksList : IPresenterGetTasksListCallback
    {
        private TasksPageViewModelBase _viewModel;

        public PresenterGetTasksList(TasksPageViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetTasksListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetTasksListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Data.Count != 0)
                {
                    PopulateData(response.Data.Data);
                    _viewModel.TextVisibility = Visibility.Collapsed;
                    _viewModel.DataGridVisibility = Visibility.Visible;
                }
                else
                {
                    _viewModel.TextVisibility = Visibility.Visible;
                    _viewModel.DataGridVisibility = Visibility.Collapsed;
                    _viewModel.ResponseString = response.Response;
                }
            });
        }

        private void PopulateData(List<Tasks> data)
        {
            foreach (var p in data)
                _viewModel.TasksList.Add(p);
        }
    }

    public abstract class TasksPageViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks>();
        public int projectId { get; set; }
        public ITaskUpdation updation { get; set; }
        public abstract void GetTasks(int projectId);

        public void CreateNewTask(Tasks pro)
        {
            CreateTask _createTask;
            _createTask = new CreateTask(new CreateTaskRequest(pro, new CancellationTokenSource()), new PresenterCreateTaskCallback(this));
            _createTask.Execute();
        }

        public void DeleteTask(int taskId)
        {
            DeleteTask _deleteTask;
            _deleteTask = new DeleteTask(new DeleteTaskRequest(taskId, new CancellationTokenSource()), new PresenterDeleteTaskCallback(this));
            _deleteTask.Execute();
        }

        public void ChangePriorityOfTask(int taskId,PriorityType priority)
        {
            ChangeTaskPriority changeTaskPriority;
           // changeTaskPriority = new ChangeTaskPriority(new ChangeTaskPriorityRequest(new CancellationTokenSource(),taskId,priority),new PresenterChangeTaskPriorityCallback(this));
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

        private Visibility _dataGridVisibility = Visibility.Collapsed;
        public Visibility DataGridVisibility
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged(nameof(DataGridVisibility));
            }
        }
    }

    public interface ITaskUpdation
    {
        void TaskUpdationNotification();
    }


    public class PresenterCreateTaskCallback : IPresenterCreateTaskCallback
    {
        private TasksPageViewModelBase _viewModel;

        public PresenterCreateTaskCallback(TasksPageViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<CreateTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<CreateTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _viewModel.ResponseString = response.Response.ToString();
                _viewModel.updation.TaskUpdationNotification();
                UIUpdation.OnTaskCreated(response.Data.Data);
                if (_viewModel.TasksList.Count != 0)
                {
                    _viewModel.DataGridVisibility = Visibility.Visible;
                    _viewModel.TextVisibility = Visibility.Collapsed;
                }
                else
                {
                    _viewModel.DataGridVisibility = Visibility.Collapsed;
                    _viewModel.TextVisibility = Visibility.Visible;
                    _viewModel.ResponseString = response.Response;
                }
            });
        }
    }


    public class PresenterDeleteTaskCallback : IPresenterDeleteTaskCallback
    {
        TasksPageViewModelBase _deleteTask;
        public PresenterDeleteTaskCallback(TasksPageViewModelBase deleteTask)
        {
            _deleteTask = deleteTask;
        }

        public void OnError(BaseException errorMessage)
        {
        }

        public void OnFailure(ZResponse<DeleteTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<DeleteTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deleteTask.ResponseString = response.Response.ToString();
                _deleteTask.updation.TaskUpdationNotification();
                UIUpdation.OnTaskDeleted(response.Data.Data);
                if (_deleteTask.TasksList.Count != 0)
                {
                    _deleteTask.DataGridVisibility = Visibility.Visible;
                    _deleteTask.TextVisibility = Visibility.Collapsed;
                }
                else
                {
                    _deleteTask.DataGridVisibility = Visibility.Collapsed;
                    _deleteTask.TextVisibility = Visibility.Visible;
                    _deleteTask.ResponseString = "Task not created yet :)";
                }
            });
        }
    }


}


