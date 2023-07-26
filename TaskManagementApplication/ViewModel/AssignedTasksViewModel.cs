﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementCleanArchitecture.ViewModel
{
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

    namespace TaskManagementCleanArchitecture.ViewModel
    {
        public class AssignedTaskPageViewModel : AssignedTasksPageViewModelBase
        {
            private GetAssignedTasksList _getTasksList;

            public override void GetTasks(int userId)
            {
                _getTasksList = new GetAssignedTasksList(new GetAssignedTasksListRequest(userId, new CancellationTokenSource()), new PresenterGetAssignedTasksList(this));
                _getTasksList.Execute();
            }
        }


        public class PresenterGetAssignedTasksList : IPresenterGetAssignedTasksListCallback
        {
            private AssignedTasksPageViewModelBase _viewModel;

            public PresenterGetAssignedTasksList(AssignedTasksPageViewModelBase viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnError(BaseException errorMessage)
            {

            }

            public void OnFailure(ZResponse<GetAssignedTasksListResponse> response)
            {

            }

            public async void OnSuccessAsync(ZResponse<GetAssignedTasksListResponse> response)
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

        public abstract class AssignedTasksPageViewModelBase : NotifyPropertyBase
        {
            public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks>();
            public int userId { get; set; }
            public ITaskUpdation updation { get; set; }
            public abstract void GetTasks(int userId);

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

    }
}