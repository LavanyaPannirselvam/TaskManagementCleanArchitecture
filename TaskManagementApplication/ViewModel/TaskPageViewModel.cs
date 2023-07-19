﻿using System;
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

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetTasksListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetTasksListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Tasks.Count > 0)
                {
                    PopulateData(response.Data.Tasks);
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

        internal void DeleteTask(int taskId)
        {
            DeleteTask _deleteTask;
            _deleteTask = new DeleteTask(new DeleteTaskRequest(taskId, new CancellationTokenSource()), new PresenterDeleteTaskCallback(this));
            _deleteTask.Execute();
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

        //public void OnLoaded()
        //{
        //    Notification.TaskCreated += OnTaskCreated;
        //}

        //private void OnTaskCreated(Tasks task)
        //{
        //    TasksList.Add(task);
        //}
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

        public void OnError(BException errorMessage)
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
                //_viewModel.NewTask = response.Data.NewTask;
                _viewModel.ResponseString = response.Response.ToString();
                _viewModel.updation.TaskUpdationNotification();
                UIUpdation.OnTaskCreated(response.Data.NewTask);
                // _viewModel.AddedView.UpdateNewProject(_viewModel.NewProject);
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

        public void OnError(BException errorMessage)
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
                //_deleteTask.Notification.NotificationMessage();//notification need to display
                _deleteTask.updation.TaskUpdationNotification();
                UIUpdation.OnTaskDeleted(response.Data.taskDeleted);
            });
        }
    }


}

