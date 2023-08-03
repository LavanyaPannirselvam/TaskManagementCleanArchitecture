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
using static TaskManagementLibrary.Domain.Usecase.DeleteTask;
using TaskManagementLibrary.Notifications;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class CreatedTasksPageViewModel : CreatedTasksPageViewModelBase
    {
        private GetCreatedTasksList _getTasksList;

        public override void GetTasks(string name,string email)
        {
            _getTasksList = new GetCreatedTasksList(new GetCreatedTasksListRequest(name,email, new CancellationTokenSource()), new PresenterGetCreatedTasksList(this));
            _getTasksList.Execute();
        }
    }


    public class PresenterGetCreatedTasksList : IPresenterGetCreatedTasksListCallback
    {
        private CreatedTasksPageViewModelBase _viewModel;

        public PresenterGetCreatedTasksList(CreatedTasksPageViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetCreatedTasksListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetCreatedTasksListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Data != null)
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

        private void PopulateData(ObservableCollection<Tasks> data)
        {
            foreach (var p in data)
                _viewModel.TasksList.Add(p);
        }
    }

    public abstract class CreatedTasksPageViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks>();
        public int userId { get; set; }
        //public ITaskUpdation updation { get; set; }
        public abstract void GetTasks(string name,string email);

        public void DeleteTask(int taskId)
        {
            DeleteTask _deleteTask;
            _deleteTask = new DeleteTask(new DeleteTaskRequest(taskId, new CancellationTokenSource()), new PresenterDeleteTaskofCreatedTasksCallback(this));
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
    }



    public class PresenterDeleteTaskofCreatedTasksCallback : IPresenterDeleteTaskCallback
    {
        CreatedTasksPageViewModelBase _deleteTask;
        public PresenterDeleteTaskofCreatedTasksCallback(CreatedTasksPageViewModelBase deleteTask)
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
                //_deleteTask.updation.TaskUpdationNotification();
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
    //public interface ITaskUpdation
    //{
    //    void TaskUpdationNotification();
    //}

}
