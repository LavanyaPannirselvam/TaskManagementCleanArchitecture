using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class RemoveTaskFromUserViewModel : RemoveTaskFromUserViewModelBase
    {
        RemoveTaskFromUser _removeTask;
        public override void RemoveTask(int userId, int taskId)
        {
            _removeTask = new RemoveTaskFromUser(new RemoveTaskRequest(new CancellationTokenSource(),userId, taskId),new PresenterRemoveTaskFromUserCallback(this));
            _removeTask.Execute();
        }
    }

    public class PresenterRemoveTaskFromUserCallback : IPresenterRemoveTaskFromUserCallback
    {
        private RemoveTaskFromUserViewModel _removeTask;
        public PresenterRemoveTaskFromUserCallback(RemoveTaskFromUserViewModel removeTask)
        {
            _removeTask = removeTask;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<RemoveTaskResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<RemoveTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _removeTask.NotificationVisibility = Visibility.Visible;
                _removeTask.ResponseString = response.Response.ToString();
            });
        }
    }


    public abstract class RemoveTaskFromUserViewModelBase : NotifyPropertyBase
    {
        public abstract void RemoveTask(int userId, int taskId);
        private string _responseString;
        public string ResponseString
        {
            get { return _responseString; }
            set
            {
                _responseString = value;
                OnPropertyChanged(nameof(ResponseString));
            }
        }

        private Visibility _notificationVisibility = Visibility.Collapsed;
        public Visibility NotificationVisibility
        {
            get { return _notificationVisibility; }
            set
            {
                _notificationVisibility = value;
                OnPropertyChanged(nameof(NotificationVisibility));
            }
        }
    }


    public interface IUpdateDeletion
    {
        void UpdateDeletion();
    }
}
