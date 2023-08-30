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
        private ResourceLoader _resourceLoader;

        public CreateNewIssue()
        {
            this.InitializeComponent();
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            _startDate = DateTimeOffset.Now;
            _endDate = DateTimeOffset.Now;
            prioritybox.ItemsSource = EnumConverter.EnumToStringConverter(typeof(PriorityType));
            statusbox.ItemsSource = Converter.EnumConverter.EnumToStringConverter(typeof(StatusType));
            _enumConverter = new EnumConverter();
            statusbox.SelectedIndex = 0;
            _resourceLoader = ResourceLoader.GetForCurrentView();
            prioritybox.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != null)
            {
                if (date.Date < DateTimeOffset.Now.Date)
                {
                    ErrorMessage.Text = _resourceLoader.GetString("StartDateErrorMessage");
                    ErrorMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorMessage.Visibility = Visibility.Collapsed;
                    _startDate = (DateTimeOffset)date.Date;
                }
            }
            else
            {
                startdate.Date = _startDate;
            }
        }

        private void EndDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            if (date.Date != null)
            {
                if (date.Date < DateTimeOffset.Now.Date)
                {
                    ErrorMessage.Text = _resourceLoader.GetString("EndDateErrorMessage");
                    ErrorMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    ErrorMessage.Visibility = Visibility.Collapsed;
                    _endDate = (DateTimeOffset)date.Date;
                }
            }
            else
            {
                enddate.Date = _endDate;
            }
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
            if (IsIssueNameEmpty(_issueName))
            {
                if (_endDate >= _startDate)
                {
                    if (_startDate >= DateTimeOffset.Now.Date)
                    {
                        return new Issue(_issueName, _description, ownerName, _statusType, _priorityType, _startDate, _endDate,id);
                    }
                    else
                    {
                        ErrorMessage.Text = _resourceLoader.GetString("StartDateErrorMessage");
                        ErrorMessage.Visibility = Visibility.Visible;
                        return null;
                    }
                }
                else
                {
                    ErrorMessage.Text = _resourceLoader.GetString("EndDateErrorMessage");
                    ErrorMessage.Visibility = Visibility.Visible;
                    return null;
                }
            }
            else return null;
        }

        private bool IsIssueNameEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ErrorMessage.Text = _resourceLoader.GetString("IssueNameEmptyErrorMsg");
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
            IssueName.Text = string.Empty;
            _issueName = string.Empty;
            DescriptionBox.Text = string.Empty;
            _description = string.Empty;
            startdate.Date = DateTimeOffset.Now;
            enddate.Date = DateTimeOffset.Now;
            _startDate = DateTimeOffset.Now;//.Date;
            _endDate = DateTimeOffset.Now;//.Date;
            statusbox.SelectedIndex = 0;
            prioritybox.SelectedIndex = 0;
            ErrorMessage.Text = string.Empty;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void IssueName_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = (TextBox)sender;//should do empty and invalid data check
            if (text.Text == string.Empty || text.Text == null)
            {
                ErrorMessage.Text = _resourceLoader.GetString("IssueNameEmptyErrorMsg");
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
            _issueName = text.Text;
        }

        private void DescriptionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _description = ((TextBox)sender).Text;
        }
    }
}
