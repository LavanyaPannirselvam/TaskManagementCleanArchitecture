using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EnumConverter = TaskManagementCleanArchitecture.Converter.EnumConverter;
using User = TaskManagementLibrary.Models.User;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class TaskDetails : UserControl, ITaskDetailsNotification, IUpdateMatchingUsersOfTask
    {
        public TaskBO task;
        public ObservableCollection<User> _users;
        public static event Action<string> Notification;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        private ObservableCollection<UserBO> _userOption;
        private ObservableCollection<UserBO> _suggestedItems;
        private ObservableCollection<UserBO> _assignedUsers;
        public TaskDetailsViewModelBase _taskDetailsViewModel;
        private EnumConverter _enumConverter;
        private double _windowWidth;
        private double _windowHeight;

        public TaskDetails()
        {
            this.InitializeComponent();
            _users = new ObservableCollection<User>();
            _taskDetailsViewModel = PresenterService.GetInstance().Services.GetService<TaskDetailsViewModelBase>();
            _taskDetailsViewModel.taskDetailsNotification = this;
            _taskDetailsViewModel.updateMatchingUsers = this;
            _userOption = new ObservableCollection<UserBO>();
            _suggestedItems = new ObservableCollection<UserBO>();
            _assignedUsers = new ObservableCollection<UserBO>();
            _enumConverter = new EnumConverter();
        }

        public TaskDetails(int id)
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
        }

        public void TaskDetailsNotification()
        {
            Notification.Invoke(_taskDetailsViewModel.ResponseString);
            _taskDetailsViewModel.ResponseString = string.Empty;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StatusCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(StatusType));
            PriorityCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            UIUpdation.UserAdded += UserAdded;
            UIUpdation.UserRemoved += UserRemoved;
            UIUpdation.UserSelectedToRemove += UIUpdation_UserSelected;
            UpdateUsers += TaskDetailsPage_UpdateUsers;
            Notification += ShowNotification;
            NameBox.TextChanged += NameBox_TextChanged;
            DescriptionBox.TextChanged += DescriptionBox_TextChanged;
            StatusCombo.SelectionChanged += StatusCombo_SelectionChanged;
            PriorityCombo.SelectionChanged += PriorityCombo_SelectionChanged;
            StartdateCalender.DateChanged += StartdateCalender_DateChanged;
            EnddateCalender.DateChanged += EnddateCalender_DateChanged;
        }

        private void UIUpdation_UserSelected(UserBO obj)
        {
            _taskDetailsViewModel.RemoveTask(obj.Email, _taskDetailsViewModel.SelectedTask.Id);
        }

        private void TaskDetailsPage_UpdateUsers(ObservableCollection<UserBO> obj)
        {
            _userOption.Clear();
            _userOption = _taskDetailsViewModel.MatchingUsers;
            foreach (var user in _userOption)
            {
                if (!_assignedUsers.Contains<UserBO>(user))
                {
                    _suggestedItems.Add(user);
                }
            }
            if (_suggestedItems.Count == 0)
            {
                _suggestedItems.Add(new UserBO("No results found", string.Empty));
                //List<string> noItem = new List<string>
                //{
                //    "No results found"
                //};
                //AssignUserBox.ItemsSource = noItem;
            }
            if (AssignUserBox.Text != string.Empty && _suggestedItems.Count != 0)
            {
                AssignUserBox.ItemsSource = _suggestedItems;
            }
            else if(AssignUserBox.Text == string.Empty)
            {
                AssignUserBox.ItemsSource = null;
            }
            //else
            //{
            //    AssignUserBox.ItemsSource = noItem;
            //}
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
            AssignUserBox.IsSuggestionListOpen = false;
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
            UIUpdation.UserSelectedToRemove -= UIUpdation_UserSelected;
            Notification -= ShowNotification;
            UpdateUsers -= TaskDetailsPage_UpdateUsers;
            NameBox.TextChanged -= NameBox_TextChanged;
            DescriptionBox.TextChanged -= DescriptionBox_TextChanged;
            StatusCombo.SelectionChanged -= StatusCombo_SelectionChanged;
            PriorityCombo.SelectionChanged -= PriorityCombo_SelectionChanged;
            StartdateCalender.DateChanged -= StartdateCalender_DateChanged;
            EnddateCalender.DateChanged -= EnddateCalender_DateChanged;
            //PriorityCombo.Loaded -= PriorityCBox_Loaded;
            //StatusCombo.Loaded -= StatusCombo_Loaded;
        }

        public void UpdateMatchingUsers()
        {
            UpdateUsers.Invoke(_taskDetailsViewModel.MatchingUsers);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _assignedUsers = _taskDetailsViewModel.AssignedUsersList;
            _taskDetailsViewModel.MatchingUsers.Clear();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.Reason != AutoSuggestionBoxTextChangeReason.SuggestionChosen)
                {
                    if (sender.Text.Length != 0)
                    {
                        _suggestedItems.Clear();
                        _taskDetailsViewModel.GetMatchingUsers(sender.Text);
                    }
                }
                else
                {
                    AssignUserBox.IsSuggestionListOpen = true;
                }
            }
        }

        private void AssignUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is UserBO user)
            {
                AssignUserBox.Text = string.Empty;
                AssignUserBox.IsSuggestionListOpen = false;
                _taskDetailsViewModel.AssignTask(user.Email, _taskDetailsViewModel.SelectedTask.Id);
            }
        }

        private void AssignUserBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is UserBO user)
            {
                sender.Text = user.Name;
            }
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (text != null && text != _taskDetailsViewModel.SelectedTask.Name)// && text != _issueViewModel.SelectedIssue.Name)
            {
                _taskDetailsViewModel.ChangeName(_taskDetailsViewModel.SelectedTask.Id, text);
            }
        }

        private void PriorityCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (PriorityType)_enumConverter.ConvertBack(e.AddedItems[0], typeof(PriorityType), null, null);
            if (_taskDetailsViewModel.SelectedTask != null && _taskDetailsViewModel.SelectedTask.Priority != selectedOption)
            {
                _taskDetailsViewModel.ChangePriority(_taskDetailsViewModel.SelectedTask.Id, selectedOption);
            }
        }

        private void PriorityCBox_Loaded(object sender, RoutedEventArgs e)
        {
            PriorityCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            if (_taskDetailsViewModel.SelectedTask != null)
            {
                PriorityCombo.SelectedIndex = (int)_taskDetailsViewModel.SelectedTask.Priority;
            }
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (text != null && text != _taskDetailsViewModel.SelectedTask.Desc)
            {
                _taskDetailsViewModel.ChangeDescription(_taskDetailsViewModel.SelectedTask.Id, text.ToString());
            }
        }

        private void StatusCombo_Loaded(object sender, RoutedEventArgs e)
        {
            StatusCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(StatusType));
            if (_taskDetailsViewModel.SelectedTask != null)
            {
                StatusCombo.SelectedIndex = (int)_taskDetailsViewModel.SelectedTask.Status;
            }
        }

        private void StatusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (StatusType)_enumConverter.ConvertBack(e.AddedItems[0], typeof(StatusType), null, null);
            if (_taskDetailsViewModel.SelectedTask != null && _taskDetailsViewModel.SelectedTask.Status != selectedOption)
            {
                _taskDetailsViewModel.ChangeStatus(_taskDetailsViewModel.SelectedTask.Id, selectedOption);
            }
        }

        private void StartdateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != _taskDetailsViewModel.SelectedTask.StartDate && date.Date >= DateTimeOffset.Now.Date)
            {
                EnddateCalender.MinDate= (DateTimeOffset)date.Date;
                _taskDetailsViewModel.ChangeStartDate(_taskDetailsViewModel.SelectedTask.Id, (DateTimeOffset)date.Date);
            }
        }

        private void EnddateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != _taskDetailsViewModel.SelectedTask.EndDate && date.Date >= DateTimeOffset.Now.Date)
            {
                _taskDetailsViewModel.ChangeEndDate(_taskDetailsViewModel.SelectedTask.Id, (DateTimeOffset)date.Date);
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;
            if(_windowWidth < 750)
            {
                Scroller.Height = 670;
            }
            else if( _windowWidth > 750 && _windowHeight < 900)
            {
                Scroller.Height = 600;
            }
        }
    }
}

