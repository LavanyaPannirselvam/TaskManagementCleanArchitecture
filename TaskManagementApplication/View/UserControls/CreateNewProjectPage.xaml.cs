using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class CreateNewProjectPage : UserControl 
    {
        private string _projectName;
        private string _description;
        private PriorityType _priorityType;
        private StatusType _statusType;
        private DateTime _startDate;
        private DateTime _endDate;
        private ProjectsPage _projectsPage;
        public CreateNewProjectPage()
        {
            this.InitializeComponent();
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            //statusbox.SelectedIndex = 0;
            //prioritybox.SelectedIndex = 3;
        }

        private void ProjectName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;//should do empty and invalid data check
            if (text.Text == string.Empty || text.Text == null)
            {
                ErrorMessage.Text = "Project name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
            _projectName = text.Text;
        }

        private void Status_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            string status = e.AddedItems[0].ToString();
            _statusType = (StatusType)Enum.Parse(typeof(StatusType), status.ToUpper().Replace(" ", ""));
        }

        private void Priority_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            string priority = e.AddedItems[0].ToString();
            _priorityType = (PriorityType)Enum.Parse(typeof(PriorityType), priority.ToUpper().Replace(" ",""));
           // _priorityType = (PriorityType)Enum.Parse(typeof(PriorityType), value.ToUpper().Replace(" ", ""));
        }

        private void StartDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _startDate = date.Date.Value.DateTime;
            if(_startDate < DateTime.Today)
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
            _endDate = date.Date.Value.DateTime;
            if (_endDate < _startDate)
            {
                ErrorMessage.Text = "End date should be greater than start date";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else ErrorMessage.Visibility = Visibility.Collapsed;
        }

        public Project GetFormData(string ownerName)
        {
            if (!IsProjectNameEmpty(_projectName))
                return new Project(_projectName, _description, ownerName, _statusType, _priorityType, _startDate, _endDate);
            else return null;
        }

        private bool IsProjectNameEmpty(string name)
        {
            if (name == null || name == "" || name == string.Empty)
            {
                ErrorMessage.Text = "Project name cannot be empty";
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
            ProjectName.Text = string.Empty;
            _projectName = string.Empty;
            DescriptionBox.Text = string.Empty;
            _description = string.Empty;
            startdate.Date = DateTime.Now;
            _startDate = DateTime.Now;
            enddate.Date = DateTime.Now;
            _endDate = DateTime.Now;
            statusbox.Text = string.Empty;
            //prioritybox.PlaceholderText = "Choose Priority";
            prioritybox.Text = string.Empty;
            _priorityType = PriorityType.HIGH;
            _statusType = StatusType.OPEN;
            ErrorMessage.Text = string.Empty;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void DescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _description = ((TextBox)sender).Text;
        }
    }
}
