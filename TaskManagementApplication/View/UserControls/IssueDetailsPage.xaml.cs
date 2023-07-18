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
    public sealed partial class IssueDetailsPage : UserControl , IIssueDetailsPageNotification
    {
        public IssueDetailsViewModelBase _issueViewModel;
        public IssueBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public IssueDetailsPage()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
            _issueViewModel.issueDetailsPageNotification = this;
            _users = new ObservableCollection<User>();
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
            //_assignTaskToUserViewModel.ResponseString = msg;
            //_removeTaskFromUserViewModel.ResponseString = msg;//to check here
        }

        private void AutoSuggestBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            AssignUserBox.IsSuggestionListOpen = false;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _users = _issueViewModel.CanAssignUsersList;
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
            _users = _issueViewModel?.CanAssignUsersList;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as User;
            _issueViewModel.RemoveUserFromIssue(data.UserId,_issueViewModel.SelectedIssue.Issue.Id);
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var result = args.ChosenSuggestion;
            foreach (var user in _users)
            {
                if (user.Name.Equals(result))
                    _issueViewModel.AssignUserToIssue(user.UserId, _issueViewModel.SelectedIssue.Issue.Id);
            }
        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Notification += ShowNotification;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
        }
    }
}
