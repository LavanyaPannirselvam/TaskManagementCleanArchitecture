using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class IssueDetailsPage : UserControl , IUserIssueAssignedNotification , IUserRemovedFromIssueNotification
    {
        private AIssueViewModelBase _aIssueViewModel;
        private AssignIssueToUserViewModelBase _assignIssueToUserViewModel;
        private RemoveIssueFromUserViewModelBase _removeIssueFromUserViewModel;
        public IssueBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public IssueDetailsPage()
        {
            this.InitializeComponent();
            _aIssueViewModel = PresenterService.GetInstance().Services.GetService<AIssueViewModelBase>();
            _assignIssueToUserViewModel = PresenterService.GetInstance().Services.GetService<AssignIssueToUserViewModelBase>();
            _assignIssueToUserViewModel.assignUser = this;
            _removeIssueFromUserViewModel = PresenterService.GetInstance().Services.GetService<RemoveIssueFromUserViewModelBase>();
            _removeIssueFromUserViewModel.userRemovedNotifcation = this;
            Notification += ShowNotification;
        }

        public void UpdateIssueAssignment()
        {
            throw new NotImplementedException();
        }

        public void UserRemovedFromIssueUpdate()
        {
            throw new NotImplementedException();
        }

        private void RemoveUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is User selected)
            {

            }
        }

        private void AssignUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is User selected)
            {
                _assignIssueToUserViewModel.AssignUserToIssue(selected.UserId, _aIssueViewModel.SelectedIssue.Issue.Id);
                _aIssueViewModel.CanAssignUsersList.Clear();
            }
        }

        public void UpdateAssignment()
        {
            Notification?.Invoke(_assignIssueToUserViewModel.ResponseString);
        }

        public void UpdateDeletion()
        {
            Notification?.Invoke(_removeIssueFromUserViewModel.ResponseString);
        }

        private void ShowNotification(string msg)
        {
            NotificationControl.Show(msg, 3000);
            //_assignTaskToUserViewModel.ResponseString = msg;
            //_removeTaskFromUserViewModel.ResponseString = msg;//to check here
        }

        private void AutoSuggestBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            AssignUserBox.IsSuggestionListOpen = false;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _users = _aIssueViewModel.CanAssignUsersList;
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
            _users = _aIssueViewModel?.CanAssignUsersList;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var result = args.ChosenSuggestion;
            foreach (var user in _users)
            {
                if (user.Name.Equals(result))
                    _assignIssueToUserViewModel.AssignUserToIssue(user.UserId, _aIssueViewModel.SelectedIssue.Issue.Id);
            }
        }
    }
}
