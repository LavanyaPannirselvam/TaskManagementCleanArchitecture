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
    public class TasksViewModel : TasksViewModelBase
    {
        private GetTasksList _getTasksList;
        
        public override void GetTasks(int projectId)
        {
            _getTasksList = new GetTasksList(new GetTasksListRequest(projectId, new CancellationTokenSource()),new PresenterGetTasksList(this));
            _getTasksList.Execute();
        }
    }


    public class PresenterGetTasksList : IPresenterGetTasksListCallback
    {
        private TasksViewModelBase _viewModel;

        public PresenterGetTasksList(TasksViewModelBase viewModel)
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
                if(response.Data.Tasks.Count > 0)
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
    public abstract class TasksViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks>();
        public int projectId { get; set; }
        public abstract void GetTasks(int projectId);

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
}

