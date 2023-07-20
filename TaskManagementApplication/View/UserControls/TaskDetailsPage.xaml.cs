using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using User = TaskManagementLibrary.Models.User;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class TaskDetailsPage : UserControl, ITaskDetailsNotification
    {
        public TaskBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public TaskDetailsViewModelBase _taskDetailsViewModel;
        
        public TaskDetailsPage()
        {
            this.InitializeComponent();
            _users = new ObservableCollection<User>();
           _taskDetailsViewModel = PresenterService.GetInstance().Services.GetService<TaskDetailsViewModelBase>();
            _taskDetailsViewModel.taskDetailsNotification = this;
        }

        public TaskDetailsPage(int id)
        {
            if (_taskDetailsViewModel == null)
            {
                _taskDetailsViewModel = PresenterService.GetInstance().Services.GetService<TaskDetailsViewModelBase>();
                _taskDetailsViewModel.taskDetailsNotification = this;
            }
            _taskDetailsViewModel.GetATask(id);
        }

        private void ShowNotification(string msg)
        {
            NotificationControl.Show(msg, 3000);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _users = _taskDetailsViewModel.CanAssignUsersList;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new ObservableCollection<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var user in _users)
                {
                    var found = splitText.All((key) =>
                    {
                        return user.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(user.Name);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                if (sender.Text != string.Empty)
                    sender.ItemsSource = suitableItems;
                else sender.ItemsSource = null;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as User;
            //_removeTaskFromUserViewModel.RemoveTask(data.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
            _taskDetailsViewModel.RemoveTask(data.UserId, _taskDetailsViewModel.SelectedTask.Tasks.Id);
            //_taskDetailsViewModel.CanRemoveUsersList.Clear();

        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var result = args.ChosenSuggestion;
            foreach (var user in _users)
            {
                if(user.Name.Equals(result))
                    //_assignTaskToUserViewModel.AssignUserToTask(user.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
                    _taskDetailsViewModel.AssignTask(user.UserId, _taskDetailsViewModel.SelectedTask.Tasks.Id);
                //_taskDetailsViewModel.CanAssignUsersList.Clear();

            }
        }

        public void TaskDetailsNotification()
        {
            Notification.Invoke(_taskDetailsViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.UserAdded += UserAdded;
            UIUpdation.UserRemoved += UserRemoved;
            Notification += ShowNotification;
        }

        private void UserRemoved(ObservableCollection<User> bO)
        {
            _taskDetailsViewModel.CanAssignUsersList.Clear();
            foreach (var u in bO)
            {
                _taskDetailsViewModel.CanAssignUsersList.Add(u);
                var delete = _taskDetailsViewModel.CanRemoveUsersList.Where(user => user.UserId == u.UserId);
                _taskDetailsViewModel.CanRemoveUsersList.Remove(delete.FirstOrDefault());
            }
        }

        private void UserAdded(ObservableCollection<User> bO)
        {
            _taskDetailsViewModel.CanRemoveUsersList.Clear();
            foreach (var user in bO)
            {
                _taskDetailsViewModel.CanRemoveUsersList.Add(user);
                var delete = _taskDetailsViewModel.CanAssignUsersList.Where(u => u.UserId == user.UserId);
                _taskDetailsViewModel.CanAssignUsersList.Remove(delete.FirstOrDefault());
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.UserAdded -= UserAdded;
            UIUpdation.UserRemoved -= UserRemoved;
            Notification -= ShowNotification;
        }
    }
}
