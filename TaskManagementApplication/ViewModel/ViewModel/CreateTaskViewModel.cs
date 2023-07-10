using System;
using System.Collections.Generic;
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
    public class CreateTaskViewModel : CreateTaskViewModelBase
    {
        private CreateTask _createTask;

        public override void CreateTask(Tasks task)
        {
            _createTask = new CreateTask(new CreateTaskRequest(task,new CancellationTokenSource()),new PresenterCreateTaskCallback(this));
            _createTask.Execute();
        }
    }


    public class PresenterCreateTaskCallback : IPresenterCreateTaskCallback
    {
        private CreateTaskViewModel _viewModel;
        
        public PresenterCreateTaskCallback(CreateTaskViewModel viewModel)
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
                _viewModel.NewTask = response.Data.NewTask;
                // _viewModel.AddedView.UpdateNewProject(_viewModel.NewProject);
            });
        }
    }



    public abstract class CreateTaskViewModelBase : NotifyPropertyBase
    {
        public abstract void CreateTask(Tasks task);
        private Tasks _newTask;
        public Tasks NewTask
        {
            get { return this._newTask; }
            set
            {
                _newTask = value;
                OnPropertyChanged(nameof(NewTask));
            }
        }

        

        public ITaskAddedView AddedView { get; set; }
    }

    public interface ITaskAddedView
    {
        void UpdateNewTask(Tasks newTask);
    }

}

