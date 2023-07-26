using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public sealed partial class IssueDetailsPage : UserControl , IIssueDetailsPageNotification
    {
        public IssueDetailsViewModelBase _issueViewModel;
        public IssueBO task;
        private ObservableCollection<User> _userOption;
        private ObservableCollection<User> _assignedUsers;
        public static event Action<string> Notification;
        public IssueDetailsPage()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
            _issueViewModel.issueDetailsPageNotification = this;
            _userOption = new ObservableCollection<User>();
           // _assignedUsers = _issueViewModel.AssignedUsersList;
            var priorityList = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>();
            //prioritycombo.ItemsSource = priorityList.ToList();
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
            var data = ((FrameworkElement)sender).DataContext as User;
            _issueViewModel.RemoveUserFromIssue(data.UserId,_issueViewModel.SelectedIssue.Id);
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //var result = args.ChosenSuggestion;
            //foreach (var user in _users)
            //{
            //    if (user.Name.Equals(result))
            //        _issueViewModel.AssignUserToIssue(user.UserId, _issueViewModel.SelectedIssue.Id);
            //}
            //AssignUserBox.Text = string.Empty;
        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Notification += ShowNotification;
            UIUpdation.UserAdded += UIUpdation_UserAdded;
            UIUpdation.UserRemoved += UIUpdation_UserRemoved;
        }

        private void UIUpdation_UserRemoved(ObservableCollection<User> obj)
        {
            //_issueViewModel.CanAssignUsersList.Clear();
            //foreach (var u in obj)
            //{
            //    _issueViewModel.CanAssignUsersList.Add(u);
            //    var delete = _issueViewModel.CanRemoveUsersList.Where(user => user.UserId == u.UserId);
            //    _issueViewModel.CanRemoveUsersList.Remove(delete.FirstOrDefault());
            //}
        }

        private void UIUpdation_UserAdded(ObservableCollection<User> obj)
        {
            //_issueViewModel.CanRemoveUsersList.Clear();
            //foreach (var user in obj)
            //{
            //    _issueViewModel.CanRemoveUsersList.Add(user);
            //    var delete = _issueViewModel.CanAssignUsersList.Where(u => u.UserId == user.UserId);
            //    _issueViewModel.CanAssignUsersList.Remove(delete.FirstOrDefault());
            //}
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
            UIUpdation.UserAdded -= UIUpdation_UserAdded;
            UIUpdation.UserRemoved -= UIUpdation_UserRemoved;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //_users = _issueViewModel.CanAssignUsersList;
            _assignedUsers = _issueViewModel.AssignedUsersList;
            _issueViewModel.MatchingUsers.Clear();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new ObservableCollection<string>();
                var splitText = sender.Text;
                _issueViewModel.GetMatchingUsers(splitText);
                _userOption = _issueViewModel.MatchingUsers;
                foreach(var user in _userOption)
                {
                    if (!_assignedUsers.Contains<User>(user))
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

        //private void AutoSuggestBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        //{
        //    _users.Clear();
        //    _users = _issueViewModel?.CanAssignUsersList;
        //}
    }
}
