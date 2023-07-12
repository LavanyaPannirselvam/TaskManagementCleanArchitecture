using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class DeleteTaskViewModel : DeleteTaskViewModelBase
    {
        DeleteTask _deleteTask;
        public override void DeleteTask(int taskId)
        {
            _deleteTask = new DeleteTask(new DeleteTaskRequest(taskId,new CancellationTokenSource()),new PresenterDeleteTaskCallback(this));
            _deleteTask.Execute();
        }
    }


    public class PresenterDeleteTaskCallback : IPresenterDeleteTaskCallback
    {
        DeleteTaskViewModel _deleteTask;
        public PresenterDeleteTaskCallback(DeleteTaskViewModel deleteTask) 
        {
            _deleteTask = deleteTask;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deleteTask.ResponseString = response.Response.ToString();
                _deleteTask.Notification.NotificationMessage();//notification need to display
            });
        }
    }


    public abstract class DeleteTaskViewModelBase : NotifyPropertyBase
    {
        public abstract void DeleteTask(int taskId);
        public IDeleteTaskNotification Notification { get; set; }
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

    }

    public interface IDeleteTaskNotification
    {
        void NotificationMessage();
    }
}
