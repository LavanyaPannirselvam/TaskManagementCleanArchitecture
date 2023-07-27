using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
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
    public sealed partial class IssueDetailsPage : UserControl , IIssueDetailsPageNotification , IUpdateMatchingUsersOfIssue
    {
        public IssueDetailsViewModelBase _issueViewModel;
        private ObservableCollection<UserBO> _userOption;
        private ObservableCollection<UserBO> _suitableItems;
        private ObservableCollection<UserBO> _assignedUsers;
        public static event Action<string> Notification;
        private UserBO _selectedUser;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        public IssueDetailsPage()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
            _issueViewModel.issueDetailsPageNotification = this;
            _issueViewModel.updateMatchingUsers = this;
            _userOption = new ObservableCollection<UserBO>();
            _suitableItems = new ObservableCollection<UserBO>();
            _assignedUsers = new ObservableCollection<UserBO>();
            _selectedUser = null;
            var priorityList = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>();
        }

        public IssueDetailsPage(int id)
        {
            if(_issueViewModel == null)
            {
                _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
                _issueViewModel.issueDetailsPageNotification = this;
            }
            _issueViewModel.GetAIssue(id);
        }

        private void ShowNotification(string msg)
        {
            NotificationControl.Show(msg, 3000);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as UserBO;
            _issueViewModel.RemoveUserFromIssue(data.Email, _issueViewModel.SelectedIssue.Id);
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //var result = args.ChosenSuggestion;
            //foreach (var user in _suitableItems)//Problem
            //{
            //    if (user.Equals(result))
            //        _issueViewModel.AssignUserToIssue(user, _issueViewModel.SelectedIssue.Id);
            //}
            _issueViewModel.AssignUserToIssue(_selectedUser.Email,_issueViewModel.SelectedIssue.Id);
            _selectedUser = null;
            AssignUserBox.Text = string.Empty;
        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Notification += ShowNotification;
            UpdateUsers += IssueDetailsPage_UpdateUsers;
            UIUpdation.UserAdded += UIUpdation_UserAdded;
            UIUpdation.UserRemoved += UIUpdation_UserRemoved;
        }

        private void IssueDetailsPage_UpdateUsers(ObservableCollection<UserBO> obj)
        {
            _userOption.Clear();
            _userOption = _issueViewModel.MatchingUsers;
            //_assignedUsers.Clear();
            //_assignedUsers = _issueViewModel.AssignedUsersList;
            foreach (var user in _userOption)
            {
                if (!_assignedUsers.Contains<UserBO>(user))
                {
                    _suitableItems.Add(user);
                }
            }
            if (_suitableItems.Count == 0)
            {
                //_suitableItems.Add("No results found");
                _suitableItems.Add(new UserBO("No results found", string.Empty));
                //AssignUserBox.ItemsSource = "No results found";
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

        private void UIUpdation_UserRemoved(ObservableCollection<UserBO> obj)
        {
            //_issueViewModel.CanAssignUsersList.Clear();
            //foreach (var u in obj)
            //{
            //    _issueViewModel.CanAssignUsersList.Add(u);
            //    var delete = _issueViewModel.CanRemoveUsersList.Where(user => user.UserId == u.UserId);
            //    _issueViewModel.CanRemoveUsersList.Remove(delete.FirstOrDefault());
            //}
            _issueViewModel.AssignedUsersList.Clear();
            foreach (var user in obj)
            {
                _issueViewModel.AssignedUsersList.Add(user);
            }
        }

        private void UIUpdation_UserAdded(ObservableCollection<UserBO> obj)
        {
            //_issueViewModel.CanRemoveUsersList.Clear();
            //foreach (var user in obj)
            //{
            //    _issueViewModel.CanRemoveUsersList.Add(user);
            //    var delete = _issueViewModel.CanAssignUsersList.Where(u => u.UserId == user.UserId);
            //    _issueViewModel.CanAssignUsersList.Remove(delete.FirstOrDefault());
            //}


            _issueViewModel.AssignedUsersList.Clear();
            foreach (var user in obj)
            {
                _issueViewModel.AssignedUsersList.Add(user);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
            UpdateUsers -= IssueDetailsPage_UpdateUsers;
            UIUpdation.UserAdded -= UIUpdation_UserAdded;
            UIUpdation.UserRemoved -= UIUpdation_UserRemoved;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //_users = _issueViewModel.CanAssignUsersList;
            _assignedUsers = _issueViewModel.AssignedUsersList;
            _issueViewModel.MatchingUsers.Clear();
            _suitableItems.Clear();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var splitText = sender.Text;
                _issueViewModel.GetMatchingUsers(splitText);
                //_userOption = _issueViewModel.MatchingUsers;
                //foreach(var user in _userOption)
                //{
                //    if (!_assignedUsers.Contains<User>(user))
                //    {
                //        suitableItems.Add(user.Name);
                //    }
                //}
                //if (suitableItems.Count == 0)
                //{
                //    suitableItems.Add("No results found");
                //}
                //if (sender.Text != string.Empty)
                //    sender.ItemsSource = suitableItems;
                //else sender.ItemsSource = null;
            }
        }

        public void UpdateMatchingUsers()
        {
            UpdateUsers.Invoke(_issueViewModel.MatchingUsers);
        }

        private void AssignUserBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            _selectedUser = args.SelectedItem as UserBO;
            sender.Text = _selectedUser.Name;
        }
    }
}
