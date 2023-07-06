using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Models.Enums;
using Windows.UI.Xaml;

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

        private void PopulateData(TaskBO data)
        {
            //TODO : if no users,msg that no users were assigned yet
            _aTaskViewModel.SelectedTask = data;
            _aTaskViewModel.ATask.Add(data);
        }
    }
    public abstract class ATaskViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<TaskBO> ATask = new ObservableCollection<TaskBO>();
        public abstract void GetATask(int taskId);

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

        private Visibility _listVisibility = Visibility.Visible;

        public Visibility ListVisibility
        {
            get { return _listVisibility; }
            set 
            { 
                _listVisibility = value;
                OnPropertyChanged(nameof(ListVisibility));
            }
            
        }

    }
}
