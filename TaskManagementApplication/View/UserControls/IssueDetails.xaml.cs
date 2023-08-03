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
    public sealed partial class IssueDetails : UserControl , IIssueDetailsPageNotification , IUpdateMatchingUsersOfIssue
    {
        public IssueDetailsViewModelBase _issueViewModel;
        private ObservableCollection<UserBO> _userOption;
        private ObservableCollection<UserBO> _suitableItems;
        private ObservableCollection<UserBO> _assignedUsers;
        public static event Action<string> Notification;
        private UserBO _selectedUser;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        //public static event Action<UserBO> RemoveUser; 
        public IssueDetails()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
            _issueViewModel.issueDetailsPageNotification = this;
            _issueViewModel.updateMatchingUsers = this;
            _userOption = new ObservableCollection<UserBO>();
            _suitableItems = new ObservableCollection<UserBO>();
            _assignedUsers = new ObservableCollection<UserBO>();
            _selectedUser = null;
            //var priorityList = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>();
           
        }

        public IssueDetails(int id)
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

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //if (_selectedUser == null)
            //{
            //    ErrorMessage.Text = "User doesnot exists";
            //    ErrorMessage.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ErrorMessage.Visibility = Visibility.Collapsed;
            //    _issueViewModel.AssignUserToIssue(_selectedUser.Email, _issueViewModel.SelectedIssue.Id);
            //}
            //_selectedUser = null;
            AssignUserBox.Text = args.QueryText;

        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PriorityCBox.ItemsSource = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>();
            PriorityCBox.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            StatusCBox.ItemsSource = Enum.GetValues(typeof (StatusType)).Cast<StatusType>();
            StatusCBox.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            Notification += ShowNotification;
            UpdateUsers += IssueDetailsPage_UpdateUsers;
            UIUpdation.UserAdded += UIUpdation_UserAdded;
            UIUpdation.UserRemoved += UIUpdation_UserRemoved;
            UIUpdation.UserSelectedToRemove += UIUpdation_UserSelected;
            UIUpdation.PriorityChanged += UIUpdation_PriorityChanged;
            
        }

        private void UIUpdation_PriorityChanged(PriorityType obj)
        {
            _issueViewModel.SelectedIssue.Priority = obj;
        }

        private void UIUpdation_UserSelected(UserBO obj)
        {
            _issueViewModel.RemoveUserFromIssue(obj.Email,_issueViewModel.SelectedIssue.Id);
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
            _issueViewModel.AssignedUsersList.Clear();
            foreach (var user in obj)
            {
                _issueViewModel.AssignedUsersList.Add(user);
            }
        }

        private void UIUpdation_UserAdded(ObservableCollection<UserBO> obj)
        {
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
            UIUpdation.UserSelectedToRemove -= UIUpdation_UserSelected;
            UIUpdation.PriorityChanged -= UIUpdation_PriorityChanged;
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

        private void PriorityCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _issueViewModel.ChangePriority(_issueViewModel.SelectedIssue.Id,(PriorityType)PriorityCBox.SelectedItem);
        }

        private void PriorityCBox_Loaded(object sender, RoutedEventArgs e)
        {
            PriorityCBox.SelectionChanged += PriorityCBox_SelectionChanged;;
        }

        
    }
}
