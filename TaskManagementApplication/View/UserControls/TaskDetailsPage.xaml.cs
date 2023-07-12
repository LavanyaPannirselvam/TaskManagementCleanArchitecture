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
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
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
    public sealed partial class TaskDetailsPage : UserControl, IUserAssignedNotification
    {
        private ATaskViewModelBase _aTaskViewModel;
        private AssignTaskToUserViewModelBase _assignTaskToUserViewModel;
        private RemoveTaskFromUserViewModelBase _removeTaskFromUserViewModel;
        public TaskBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public TaskDetailsPage()
        {
            this.InitializeComponent();
            _aTaskViewModel = PresenterService.GetInstance().Services.GetService<ATaskViewModelBase>();
            _assignTaskToUserViewModel = PresenterService.GetInstance().Services.GetService<AssignTaskToUserViewModelBase>();
            _removeTaskFromUserViewModel = PresenterService.GetInstance().Services.GetService<RemoveTaskFromUserViewModelBase>();
            _users = new ObservableCollection<User>();
            //task = _aTaskViewModel.ATask.FirstOrDefault();
            //if (task!=null && task.AssignedUsers.Count!= 0)
            //{
            //    TextVisibility = Visibility.Collapsed;
            //    ListVisibility = Visibility.Visible;
            //}

        }
        private void RemoveUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is User selected)
            {
                _removeTaskFromUserViewModel.RemoveTask(selected.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
                _aTaskViewModel.CanRemoveUsersList.Clear();
            }
        }

        private void AssignUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is User selected)
            {
                _assignTaskToUserViewModel.AssignUserToTask(selected.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
                _aTaskViewModel.CanAssignUsersList.Clear();
            }
        }

        public void SuccessNotification()
        {
            Notification?.Invoke(_aTaskViewModel.ResponseString);//ithuku aprm therla
        }

        private void AutoSuggestBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            AssignUserBox.IsSuggestionListOpen = false;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _users = _aTaskViewModel.CanAssignUsersList;
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
                sender.ItemsSource = suitableItems;
            }
            
        }

       
        private void AutoSuggestBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _users.Clear();
            _users = _aTaskViewModel?.CanAssignUsersList;
        }
    }
}
