using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }
       
        private void ProjectName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;//should do empty and invalid data check
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
        }

        private void StartDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _startDate = date.Date.Value.DateTime;
        }

        private void EndDate_DataChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = (CalendarDatePicker)sender;
            _endDate = date.Date.Value.DateTime;
        }

        public Project GetFormData(string ownerName)
        {
            return new Project(_projectName,_description,ownerName,_statusType,_priorityType,_startDate,_endDate);
        }

        public void ClearFormData()
        {
            ProjectName.Text = string.Empty;
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            prioritybox.Text = string.Empty;
            statusbox.Text = string.Empty;
        }

        public void UpdateNewTask(Project newProject)
        {
        }
    }
}
