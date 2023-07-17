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
    public sealed partial class CreateNewIssuePage : UserControl , INotifyPropertyChanged
    {
        private string _issueName;
        private string _description;
        private PriorityType _priorityType;
        private StatusType _statusType;
        private DateTime _startDate;
        private DateTime _endDate;
        private CreateIssueViewModelBase _createIssueViewModelBase;
        public CreateNewIssuePage()
        {
            this.InitializeComponent();
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            _createIssueViewModelBase = PresenterService.GetInstance().Services.GetService<CreateIssueViewModelBase>();
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

        private Visibility _descriptionTextBoxVisibility;
        public Visibility DescriptionTextBoxVisibility
        {
            get { return _descriptionTextBoxVisibility; }
            set
            {
                _descriptionTextBoxVisibility = value;
                NotifyPropertyChanged(nameof(DescriptionTextBoxVisibility));
            }
        }

        private void IssueName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;
            _issueName = text.Text;
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

        private void Priority_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            string priority = e.AddedItems[0].ToString();
            _priorityType = (PriorityType)Enum.Parse(typeof(PriorityType), priority.ToUpper().Replace(" ", ""));
        }

        private void Status_Selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            string status = e.AddedItems[0].ToString();
            _statusType = (StatusType)Enum.Parse(typeof(StatusType), status.ToUpper().Replace(" ", ""));
        }

        public Issue GetFormData(string ownerName, int id)
        {
            return new Issue(_issueName, _description, ownerName, _statusType, _priorityType, _startDate, _endDate, id);
        }

        public void ClearFormData()
        {
            IssueName.Text = string.Empty;
            startdate.Date = DateTime.Now;
            enddate.Date = DateTime.Now;
            prioritybox.Text = string.Empty;
            statusbox.Text = string.Empty;
        }
    }
}
