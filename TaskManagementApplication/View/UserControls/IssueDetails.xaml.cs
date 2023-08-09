using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using static System.Net.Mime.MediaTypeNames;

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
        private readonly TextInfo _myTI = new CultureInfo("en-US", false).TextInfo;
        private UserBO _selectedUser;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        
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
           // AssignUserBox.Text = args.QueryText;

        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
            _issueViewModel.ResponseString = string.Empty;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Notification += ShowNotification;
            UpdateUsers += IssueDetailsPage_UpdateUsers;
            UIUpdation.UserAdded += UIUpdation_UserAdded;
            UIUpdation.UserRemoved += UIUpdation_UserRemoved;
            UIUpdation.UserSelectedToRemove += UIUpdation_UserSelected;
        }

      
        private void UIUpdation_UserSelected(UserBO obj)
        {
            _issueViewModel.RemoveUserFromIssue(obj.Email,_issueViewModel.SelectedIssue.Id);
        }

        private void IssueDetailsPage_UpdateUsers(ObservableCollection<UserBO> obj)
        {
            _userOption.Clear();
            _userOption = _issueViewModel.MatchingUsers;
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
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _assignedUsers = _issueViewModel.AssignedUsersList;
            _issueViewModel.MatchingUsers.Clear();
            _suitableItems.Clear();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.Reason != AutoSuggestionBoxTextChangeReason.SuggestionChosen)
                {
                    if (sender.Text.Length != 0)
                    {
                        _issueViewModel.GetMatchingUsers(sender.Text);
                    }
                }
                else
                {
                    AssignUserBox.IsSuggestionListOpen = true;
                }
            }
        }

        public void UpdateMatchingUsers()
        {
            UpdateUsers.Invoke(_issueViewModel.MatchingUsers);
        }

        //private void AssignUserBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    _selectedUser = args.SelectedItem as UserBO;
        //    //AssignUserBox.TextMemberPath = _selectedUser.Name.ToString();
        //}

        private void PriorityCBox_Loaded(object sender, RoutedEventArgs e)
        {
            PriorityCombo.ItemsSource = Enum.GetValues(typeof(PriorityType)).Cast<PriorityType>();
            PriorityCombo.SelectedIndex = 0;
        }

        private void StatusCombo_Loaded(object sender, RoutedEventArgs e)
        {
            StatusCombo.ItemsSource = Enum.GetValues(typeof (StatusType)).Cast<StatusType>();
            StatusCombo.SelectedIndex = 0;
            //StatusCombo.SelectedItem = null;
        }

        private void StatusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //StatusCombo.SelectedItem = _issueViewModel.SelectedIssue.Status;
            if (_issueViewModel.SelectedIssue != null &&  _issueViewModel.SelectedIssue.Status != (StatusType)e.AddedItems[0])
            {
                _issueViewModel.ChangeStatus(_issueViewModel.SelectedIssue.Id, (StatusType)StatusCombo.SelectedItem);
            }
        }

        private void PriorityCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PriorityCombo.SelectedItem = _issueViewModel?.SelectedIssue.Priority;
            if (_issueViewModel.SelectedIssue != null && _issueViewModel.SelectedIssue.Priority != (PriorityType)e.AddedItems[0])
            {
                _issueViewModel.ChangePriority(_issueViewModel.SelectedIssue.Id, (PriorityType)PriorityCombo.SelectedItem);
            }
        }

        private void StartdateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if(date.Date > _issueViewModel.SelectedIssue.StartDate) 
            {
                _issueViewModel.ChangeStartDate(_issueViewModel.SelectedIssue.Id, (DateTimeOffset)date.Date);
            }
        }

        private void EnddateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date > _issueViewModel.SelectedIssue.EndDate)
            {
                _issueViewModel.ChangeEndDate(_issueViewModel.SelectedIssue.Id, (DateTimeOffset)date.Date);
            }
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var text = (TextBox)sender;
            //if (text.ToString() != string.Empty && text.ToString() != _issueViewModel.SelectedIssue.Desc)
            //{
            //    _issueViewModel.ChangeDescription(_issueViewModel.SelectedIssue.Id, text.ToString());
            //}
        }

       // private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
       //{
       //     var text = (TextBox)e.OriginalSource;
       //     if (text != null)// && text != _issueViewModel.SelectedIssue.Name)
       //     {
       //     }
       // }

        private void DescriptionBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            var newText = args.NewText;
            var oldText = sender.Text; if (newText != oldText && oldText != "" && newText != "")
            {
                _issueViewModel.ChangeDescription(_issueViewModel.SelectedIssue.Id, newText);
            }
        }
    }
}
