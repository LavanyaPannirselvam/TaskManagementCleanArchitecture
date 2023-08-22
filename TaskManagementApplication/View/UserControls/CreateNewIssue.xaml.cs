using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.Converter;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class CreateNewIssue : UserControl , INotifyPropertyChanged
    {
        private string _issueName;
        private string _description;
        private PriorityType _priorityType;
        private StatusType _statusType;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        private EnumConverter _enumConverter;
        private ResourceLoader resourceLoader;

        public CreateNewIssue()
        {
            this.InitializeComponent();
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            prioritybox.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            statusbox.ItemsSource = Converter.EnumConverter.EnumToStringConverter(typeof(StatusType));
            _enumConverter = new EnumConverter();
            statusbox.SelectedIndex = 0;
            resourceLoader = ResourceLoader.GetForCurrentView();
            prioritybox.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IssueName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;//should do empty and invalid data check
            if (text.Text == string.Empty || text.Text == null)
            {
                ErrorMessage.Text = resourceLoader.GetString("IssueNameEmptyErrorMsg");
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
            _issueName = text.Text;
        }

        private void descriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _description = ((TextBox)sender).Text;
        }

        private void StartDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _startDate = (DateTimeOffset)date.Date;//.Value.DateTime;
            if (_startDate < DateTime.Today)
            {
                //ErrorMessage.Text = "Start date cannot be yesterday";
                //ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = resourceLoader.GetString("StartDateErrorMessage");
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
                ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void EndDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _endDate = (DateTimeOffset)date.Date;//.Value.DateTime;
            if (_endDate < _startDate)
            {
                ErrorMessage.Text = resourceLoader.GetString("EndDateErrorMessage");
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void Priority_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
           _priorityType = (PriorityType)_enumConverter.ConvertBack(e.AddedItems[0], typeof(PriorityType),null,null);
        }

        private void Status_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            _statusType = (StatusType)_enumConverter.ConvertBack(e.AddedItems[0].ToString(), typeof(StatusType),null,null);
        }

        public Issue GetFormData(string ownerName, int id)
        {
            if (!IsIssueNameEmpty(_issueName))
                return new Issue(_issueName, _description, ownerName, _statusType, _priorityType, _startDate, _endDate,id);
            else return null;
        }

        private bool IsIssueNameEmpty(string name)
        {
            if (name == null || name == "" || name == string.Empty)
            {
                ErrorMessage.Text = resourceLoader.GetString("IssueNameEmptyErrorMsg");
                ErrorMessage.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        public void ClearFormData()
        {
            IssueName.Text = string.Empty;
            _issueName = string.Empty;
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
    }
}
