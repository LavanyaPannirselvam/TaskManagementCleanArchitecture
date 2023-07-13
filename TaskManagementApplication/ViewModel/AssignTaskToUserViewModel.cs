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
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class AssignTaskToUserViewModel : AssignTaskToUserViewModelBase
    {
        AssignTaskToUser _assignTaskToUser;
        public override void AssignUserToTask(int userId, int taskId)
        {
            _assignTaskToUser = new AssignTaskToUser(new AssignTaskRequest(taskId, userId, new CancellationTokenSource()), new PresenterAssignTaskCallback(this));
            _assignTaskToUser.Execute();
        }
    }

    public class PresenterAssignTaskCallback : IPresenterAssignTaskCallback
    {
        private AssignTaskToUserViewModel _assignTaskToUser;

        public PresenterAssignTaskCallback(AssignTaskToUserViewModel assignTaskToUser)
        {
            _assignTaskToUser = assignTaskToUser;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<AssignTaskResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<AssignTaskResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _assignTaskToUser.NotificationVisibility = Visibility.Visible;//as event
                _assignTaskToUser.ResponseString = response.Response.ToString();
                _assignTaskToUser.assignUser.UpdateAssignment();
            });
        }

        //private void PopulateData(List<User> data)
        //{
        //    foreach (var p in data)
        //    {
        //        _assignTaskToUser.UsersList.Add(p);
        //    }
        //}
    }

    public abstract class AssignTaskToUserViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public IUserAssignedNotification assignUser { get; set; }
        public abstract void AssignUserToTask(int userId, int taskId);

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


    public interface IUserAssignedNotification
    {
        void UpdateAssignment();
    }
}

