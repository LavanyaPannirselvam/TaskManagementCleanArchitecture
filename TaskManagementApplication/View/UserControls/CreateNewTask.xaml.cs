using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using TaskManagementCleanArchitecture.Converter;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
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
using EnumConverter = TaskManagementCleanArchitecture.Converter.EnumConverter;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class CreateNewTask : UserControl,INotifyPropertyChanged
    {
        private string _taskName;
        private string _description;
        private PriorityType _priorityType;
        private StatusType _statusType;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        EnumConverter _enumConverter;
        public CreateNewTask()
        {
            this.InitializeComponent();
            startdate.Date = DateTimeOffset.Now.Date;
            enddate.Date = DateTimeOffset.Now.Date;
            prioritybox.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            statusbox.ItemsSource = Converter.EnumConverter.EnumToStringConverter(typeof(StatusType));
            _enumConverter = new EnumConverter();
            statusbox.SelectedIndex = 0;
            prioritybox.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Visibility _nameTextBoxVisibility;
        public Visibility NameTextBoxVisibility
        {
            get { return _nameTextBoxVisibility; }
            set
            {
                _nameTextBoxVisibility = value;
                NotifyPropertyChanged(nameof(NameTextBoxVisibility));
            }
        }

        private void TaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;//should do empty and invalid data check
            if (text.Text == string.Empty || text.Text == null)
            {
                ErrorMessage.Text = "Task name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
            _taskName = text.Text;
        }

        private void StartDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _startDate = (DateTimeOffset)date.Date;//.Value.DateT;
            if (_startDate < DateTime.Today)
            {
                ErrorMessage.Text = "Start date cannot be yesterday";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
                ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void EndDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _endDate = (DateTimeOffset)date.Date;
            if (_endDate < _startDate)
            {
                ErrorMessage.Text = "End date should be greater than start date";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void Priority_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            _priorityType = (PriorityType)_enumConverter.ConvertBack(e.AddedItems[0].ToString(), typeof(PriorityType),null,null);
        }

        private void Status_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            _statusType = (StatusType)_enumConverter.ConvertBack(e.AddedItems[0].ToString(), typeof(StatusType),null,null);
        }

        public Tasks GetFormData(string ownerName,int id)
        {
            if (IsTaskNameEmpty(_taskName))
                return new Tasks(_taskName, _description, ownerName, _statusType, _priorityType, _startDate, _endDate, id);
            else return null;
        }

        private bool IsTaskNameEmpty(string name)
        {
            if (name == null || name == "" || name == string.Empty)
            {
                ErrorMessage.Text = "Task name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
                return true;
            }
        }

        public void ClearFormData()
        {
            TaskName.Text = string.Empty;
            _taskName = string.Empty;
            DescriptionBox.Text = string.Empty;
            _description = string.Empty;
            startdate.Date = DateTimeOffset.Now.Date;
            _startDate = DateTimeOffset.Now.Date;
            enddate.Date = DateTimeOffset.Now.Date;
            _endDate = DateTimeOffset.Now.Date;
            statusbox.SelectedIndex = 0;
            prioritybox.SelectedIndex = 0;
            ErrorMessage.Text = string.Empty;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _description = ((TextBox)sender).Text;
        }
    }
}
