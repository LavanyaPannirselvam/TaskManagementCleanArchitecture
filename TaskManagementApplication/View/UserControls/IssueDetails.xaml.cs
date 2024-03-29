﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.Converter;
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
using EnumConverter = TaskManagementCleanArchitecture.Converter.EnumConverter;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
   
    public sealed partial class IssueDetails : UserControl, IIssueDetailsPageNotification, IUpdateMatchingUsersOfIssue
    {
        public IssueDetailsViewModelBase _issueViewModel;
        private ObservableCollection<UserBO> _userOption;
        private ObservableCollection<UserBO> _suggestedItems;
        private ObservableCollection<UserBO> _assignedUsers;
        public static event Action<string> Notification;
        public static event Action<ObservableCollection<UserBO>> UpdateUsers;
        private EnumConverter _enumConverter;
        private double _windowWidth;
        private double _windowHeight;

        public IssueDetails()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssueDetailsViewModelBase>();
            _issueViewModel.issueDetailsPageNotification = this;
            _issueViewModel.updateMatchingUsers = this;
            _userOption = new ObservableCollection<UserBO>();
            _suggestedItems = new ObservableCollection<UserBO>();
            _assignedUsers = new ObservableCollection<UserBO>();
            _enumConverter = new EnumConverter();
        }

        public IssueDetails(int id)
        {
            if (_issueViewModel == null)
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
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is UserBO user)
            {
                AssignUserBox.Text = string.Empty;
                AssignUserBox.IsSuggestionListOpen = false;
                _issueViewModel.AssignIssueToUser(user.Email, _issueViewModel.SelectedIssue.Id);
            }
        }

        public void IssueDetailsPageNotification()
        {
            Notification.Invoke(_issueViewModel.ResponseString);
            _issueViewModel.ResponseString = string.Empty;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PriorityCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            StatusCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(StatusType));
            Notification += ShowNotification;
            UpdateUsers += IssueDetailsPage_UpdateUsers;
            UIUpdation.UserAdded += UIUpdation_UserAdded;
            UIUpdation.UserRemoved += UIUpdation_UserRemoved;
            UIUpdation.UserSelectedToRemove += UIUpdation_UserSelected;
        }

        private void UIUpdation_UserSelected(UserBO obj)
        {
            _issueViewModel.RemoveUserFromIssue(obj.Email, _issueViewModel.SelectedIssue.Id);
        }

        private void IssueDetailsPage_UpdateUsers(ObservableCollection<UserBO> obj)
        {
            _userOption.Clear();
            _userOption = _issueViewModel.MatchingUsers;
            foreach (var user in _userOption)
            {
                if (!_assignedUsers.Contains<UserBO>(user))
                {
                    _suggestedItems.Add(user);
                }
            }
            if (_suggestedItems.Count == 0)
            {
                AssignUserBox.ItemsSource = new List<string> { "No results found" };
            }
            if (AssignUserBox.Text != string.Empty && _suggestedItems.Count != 0)
            {
                AssignUserBox.ItemsSource = _suggestedItems;
            }
            else if (AssignUserBox.Text == string.Empty)
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
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.Reason != AutoSuggestionBoxTextChangeReason.SuggestionChosen)
                {
                    if (sender.Text.Length != 0)
                    {
                        _suggestedItems.Clear();
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

        //private void PriorityCBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    PriorityCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
        //    if (_issueViewModel.SelectedIssue != null)
        //    {
        //        StatusCombo.SelectedIndex = (int)_issueViewModel.SelectedIssue.Status;
        //    }
        //}

        //private void StatusCombo_Loaded(object sender, RoutedEventArgs e)
        //{
        //    StatusCombo.ItemsSource = EnumConverter.EnumToStringConverter(typeof(StatusType));
        //    if (_issueViewModel.SelectedIssue != null)
        //    {
        //        StatusCombo.SelectedIndex = (int)_issueViewModel.SelectedIssue.Status;
        //    }
        //}

        private void StatusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (StatusType)_enumConverter.ConvertBack(e.AddedItems[0], typeof(StatusType), null, null);
            if (_issueViewModel.SelectedIssue != null && _issueViewModel.SelectedIssue.Status != selectedOption)
            {
                _issueViewModel.ChangeStatus(_issueViewModel.SelectedIssue.Id, selectedOption);
            }
        }

        private void PriorityCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (PriorityType)_enumConverter.ConvertBack(e.AddedItems[0], typeof(PriorityType), null, null);
            if (_issueViewModel.SelectedIssue != null && _issueViewModel.SelectedIssue.Priority != selectedOption)
            {
                _issueViewModel.ChangePriority(_issueViewModel.SelectedIssue.Id, selectedOption);
            }
        }

        private void StartdateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != _issueViewModel.SelectedIssue.StartDate && date.Date >= DateTimeOffset.Now.Date)
            {
                EnddateCalender.MinDate = (DateTimeOffset)date.Date;
                _issueViewModel.ChangeStartDate(_issueViewModel.SelectedIssue.Id, (DateTimeOffset)date.Date);
            }
        }

        private void EnddateCalender_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != _issueViewModel.SelectedIssue.EndDate && date.Date >= DateTimeOffset.Now.Date)
            {
                _issueViewModel.ChangeEndDate(_issueViewModel.SelectedIssue.Id, (DateTimeOffset)date.Date);
            }
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (text != null && text != _issueViewModel.SelectedIssue.Desc)
            {
                _issueViewModel.ChangeDescription(_issueViewModel.SelectedIssue.Id, text.ToString());
            }
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (text != null && text!= _issueViewModel.SelectedIssue.Name)
            {
                _issueViewModel.ChangeName(_issueViewModel.SelectedIssue.Id, text);
            }
        }

        private void AssignUserBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem is UserBO user)
            {
                sender.Text = user.Name;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;
            if (_windowWidth < 750)
            {
                Scroller.Height = 670;
            }
            else if (_windowWidth > 750 && _windowHeight < 900)
            {
                Scroller.Height = 670;
            }
        }

        private void AssignUserBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignUserBox.Text = string.Empty;
            AssignUserBox.IsSuggestionListOpen = false;
        }
    }
}
