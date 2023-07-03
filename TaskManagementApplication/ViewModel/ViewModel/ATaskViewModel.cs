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
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class ATaskViewModel : ATaskViewModelBase
    {
        private GetATask _getATask;
        public override void GetATask(int projectId)
        {
            _getATask = new GetATask(new GetATaskRequest(projectId, new CancellationTokenSource()), new PresenterGetATask(this));
            _getATask.Execute();
        }
    }


    public class PresenterGetATask : IPresenterGetATaskListCallback
    {
        private readonly ATaskViewModel _aTaskViewModel;

        public PresenterGetATask(ATaskViewModel viewModel)
        {
            _aTaskViewModel = viewModel;
        }

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetATaskResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetATaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                PopulateData(response.Data.task);
            });
        }

        private void PopulateData(Tasks data)
        {
            //TODO : if no users,msg that no users were assigned yet
            _aTaskViewModel.ATask.Add(data);
        }
    }
    public abstract class ATaskViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Tasks> ATask = new ObservableCollection<Tasks>();
        public abstract void GetATask(int taskId);
    }
}
