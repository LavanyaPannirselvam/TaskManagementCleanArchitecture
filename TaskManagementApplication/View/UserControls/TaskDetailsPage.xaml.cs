using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class TaskDetailsPage : UserControl, ITaskDetailsNotification, IUpdateMatchingUsersOfTask
    {
        public TaskBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        private ObservableCollection<UserBO> _userOption;
        private ObservableCollection<UserBO> _suitableItems;
        private ObservableCollection<UserBO> _assignedUsers;
        private UserBO _selectedUser;
        public TaskDetailsViewModelBase _taskDetailsViewModel;

        public TaskDetailsPage()
        {
            this.InitializeComponent();
            _users = new ObservableCollection<User>();
            _taskDetailsViewModel = PresenterService.GetInstance().Services.GetService<TaskDetailsViewModelBase>();
            _taskDetailsViewModel.taskDetailsNotification = this;
            _taskDetailsViewModel.updateMatchingUsers = this;
            _userOption = new ObservableCollection<UserBO>();
            _suitableItems = new ObservableCollection<UserBO>();
            _assignedUsers = new ObservableCollection<UserBO>();
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
            _assignedUsers = _taskDetailsViewModel.AssignedUsersList;
            _taskDetailsViewModel.MatchingUsers.Clear();
            _suitableItems.Clear();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var splitText = sender.Text;
                _taskDetailsViewModel.GetMatchingUsers(splitText);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as UserBO;
            _taskDetailsViewModel.RemoveTask(data.Email, _taskDetailsViewModel.SelectedTask.Id);
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _taskDetailsViewModel.AssignTask(_selectedUser.Email, _taskDetailsViewModel.SelectedTask.Id);
            _selectedUser = null;
            AssignUserBox.Text = string.Empty;
        }

        public void TaskDetailsNotification()
        {
            Notification.Invoke(_taskDetailsViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.UserAdded += UserAdded;
            UIUpdation.UserRemoved += UserRemoved;
            UIUpdation.UserSelected += UIUpdation_UserSelected;
            UpdateUsers += TaskDetailsPage_UpdateUsers;
            Notification += ShowNotification;
        }

        private void UIUpdation_UserSelected(UserBO obj)
        {
            _taskDetailsViewModel.RemoveTask(obj.Email,_taskDetailsViewModel.SelectedTask.Id);
        }

        private void TaskDetailsPage_UpdateUsers(ObservableCollection<UserBO> obj)
        {
            _userOption.Clear();
            _userOption = _taskDetailsViewModel.MatchingUsers;
            foreach (var user in _userOption)
            {
                if (!_assignedUsers.Contains<UserBO>(user))
                {
                    _suitableItems.Add(user);
                }
            }
            if (_suitableItems.Count == 0)
            {
                _suitableItems.Add(new UserBO("No results found", string.Empty));
            }
            if (AssignUserBox.Text != string.Empty)
            {
                AssignUserBox.ItemsSource = _suitableItems;
            }
            else
            {
                AssignUserBox.ItemsSource = null;
            }
        }

        private void UserRemoved(ObservableCollection<UserBO> bO)
        {
            _taskDetailsViewModel.AssignedUsersList.Clear();
            foreach (var user in bO)
            {
                _taskDetailsViewModel.AssignedUsersList.Add(user);
            }
        }

        private void UserAdded(ObservableCollection<UserBO> bO)
        {
            _taskDetailsViewModel.AssignedUsersList.Clear();
            foreach (var user in bO)
            {
                _taskDetailsViewModel.AssignedUsersList.Add(user);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.UserAdded -= UserAdded;
            UIUpdation.UserRemoved -= UserRemoved;
            UIUpdation.UserSelected -= UIUpdation_UserSelected;
            Notification -= ShowNotification;
            UpdateUsers -= TaskDetailsPage_UpdateUsers;
        }

        private void AssignUserBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            _selectedUser = args.SelectedItem as UserBO;
            sender.Text = _selectedUser.Name;
        }

        public void UpdateMatchingUsers()
        {
            UpdateUsers.Invoke(_taskDetailsViewModel.MatchingUsers);
        }
    }
}
