using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class TasksViewModel : TaskViewModelBase
    {
        private GetTasksList _getTasksList;
        public override void GetTasksList(int projectId)
        {
            _getTasksList = new GetTasksList(new GetTasksListRequest(projectId, new CancellationTokenSource()),new PresenterGetTasksList(this));
            _getTasksList.Execute();
        }
    }


    public class PresenterGetTasksList : IPresenterGetTasksListCallback
    {
        private TaskViewModelBase _viewModel;

        public PresenterGetTasksList(TaskViewModelBase viewModel)
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
                PopulateData(response.Data.Tasks);
            });
        }

        private void PopulateData(List<Tasks> data)
        {
            //TODO : if no users,msg that no users were assigned yet
            foreach (var p in data)
            {
                _viewModel.TasksList.Add(p);
            }
        }
    }
    public abstract class TaskViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks> ();
        public abstract void GetTasksList(int projectId);
    }
}
