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
                if(response.Data.task.AssignedUsers != null)//if users available
                {
                    _aTaskViewModel.ListVisibility = Visibility.Visible;
                    _aTaskViewModel.TextVisibility = Visibility.Collapsed;
                    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                    _aTaskViewModel.RemoveButtonVisibility = Visibility.Visible;
                }
                else//no users
                {
                    _aTaskViewModel.TextVisibility = Visibility.Visible;
                    _aTaskViewModel.ListVisibility = Visibility.Collapsed;
                    _aTaskViewModel.ResponseString = response.Response.ToString();
                    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                    _aTaskViewModel.RemoveButtonVisibility = Visibility.Collapsed;
                }
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

        private Visibility _listVisibility = Visibility.Collapsed;
        public Visibility ListVisibility
        {
            get { return _listVisibility; }
            set 
            { 
                _listVisibility = value;
                OnPropertyChanged(nameof(ListVisibility));
            }
            
        }

        private string _responseString = string.Empty;
        public string ResponseString
        {
            get { return _responseString; }
            set 
            { 
                _responseString =  value; 
                OnPropertyChanged(nameof(ResponseString));
            }
        }

        private Visibility _assignButtonVisibility = Visibility.Collapsed;
        public Visibility AssignButtonVisibility
        {
            get { return _assignButtonVisibility; }
            set
            {
                _assignButtonVisibility = value;
                OnPropertyChanged(nameof(AssignButtonVisibility));
            }
        }

        private Visibility _removeButtonVisibility = Visibility.Collapsed;
        public Visibility RemoveButtonVisibility
        {
            get { return _removeButtonVisibility; }
            set
            {
                _removeButtonVisibility = value;
                OnPropertyChanged(nameof(RemoveButtonVisibility));
            }
        }
    }
}
