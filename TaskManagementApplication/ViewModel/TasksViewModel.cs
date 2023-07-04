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
                PopulateData(response.Data.Tasks);
            });
        }

        private void PopulateData(List<Tasks> data)
        {
            //TODO : if no users,msg that no users were assigned yet
            foreach (var p in data)
                _viewModel.TasksList.Add(p);
        }
    }
    public abstract class TasksViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> TasksList = new ObservableCollection<Tasks>();
        public abstract void GetTasks(int projectId);

    }
}

