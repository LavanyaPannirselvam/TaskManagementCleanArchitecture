using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class IssuesPage : UserControl , IDeleteIssueNotification
    {
        private static bool _itemSelected;
        private Issue _issue = new Issue();
        public IssuesViewModelBase _issueViewModel;
        public AIssueViewModelBase _aIssueViewModel;
        public DeleteIssueViewModelBase _deleteIssueViewModel;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(CUser), typeof(LoggedInUserBO), typeof(IssuesPage), new PropertyMetadata(null));
        public static event Action<string> IssuePageNotification;
        private double _windowHeight;
        private double _windowWidth;
        private bool _narrowLayout;
        
        public LoggedInUserBO CUser
        {
            get { return (LoggedInUserBO)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public IssuesPage()
        {
            this.InitializeComponent();
            _aIssueViewModel = PresenterService.GetInstance().Services.GetService<AIssueViewModelBase>();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssuesViewModelBase>();
            _deleteIssueViewModel = PresenterService.GetInstance().Services.GetService<DeleteIssueViewModelBase>();
            _deleteIssueViewModel.Notification = this;
            IssuePageNotification += ShowIssuePageNotification;
        }

        private void NewIssueButton_Click(object sender, RoutedEventArgs e)
        {
            AddIssueForm.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - AddIssueForm.ActualWidth / 4 + 300;
            double verticalOffset = Window.Current.Bounds.Height / 2 - AddIssueForm.ActualHeight / 2 - 300;
            AddIssueForm.HorizontalOffset = horizontalOffset;
            AddIssueForm.VerticalOffset = verticalOffset;
            AddIssueForm.IsOpen = true;
        }

        private void IssueList_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
                e.Column.Header = "Issue Id";
            if (e.Column.Header.ToString() == "ProjectId")
                e.Column.Header = "Project Id";
            if (e.Column.Header.ToString() == "CreateddBy")
                e.Column.Header = "Created by";
            if (e.Column.Header.ToString() == "StartDate")
                e.Column.Header = "Start Date";
            if (e.Column.Header.ToString() == "EndDate")
                e.Column.Header = "End Date";
        }

        private void IssueOfAProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_itemSelected = true;
            //Grid.SetColumn(IssuesList, 0);
            //Grid.SetColumn(IssueGridSplitter, 1);
            //Grid.SetColumn(IssueDetailGrid, 2);  
            //Grid.SetColumnSpan(IssuesList, 1);
            //Grid.SetColumnSpan(IssueGridSplitter, 1);
            //Grid.SetColumnSpan(IssueDetailGrid, 1);
            //IssuesList.Visibility = Visibility.Visible;
            //IssueGridSplitter.Visibility = Visibility.Visible;
            //IssueDetailGrid.Visibility = Visibility.Visible;
            //if ((sender as DataGrid).SelectedItem is Issue issue)
            //{
            //    _aIssueViewModel.GetAIssue(issue.Id);
            //    _aIssueViewModel.CanAssignUsersList.Clear();
            //    _aIssueViewModel.CanRemoveUsersList.Clear();
            //    //TasksList.DataContext = _task;
            //}
            _itemSelected = true;
            if (_narrowLayout)
            {
                _narrowLayout = true;
                IssuesList.Visibility = Visibility.Collapsed;
                IssueGridSplitter.Visibility = Visibility.Collapsed;
                IssueDetailGrid.Visibility = Visibility.Visible;
                Grid.SetColumn(IssueDetailGrid, 0);
                Grid.SetColumnSpan(IssueDetailGrid, 3);
                BackToList.Visibility = Visibility.Visible;

            }
            else
            {
                _narrowLayout = false;
                Grid.SetColumn(IssuesList, 0);
                Grid.SetColumn(IssueGridSplitter, 1);
                Grid.SetColumn(IssueDetailGrid, 2);
                Grid.SetColumnSpan(IssuesList, 1);
                Grid.SetColumnSpan(IssueGridSplitter, 1);
                Grid.SetColumnSpan(IssueDetailGrid, 1);
                IssuesList.Visibility = Visibility.Visible;
                IssueGridSplitter.Visibility = Visibility.Visible;
                IssueDetailGrid.Visibility = Visibility.Visible;

            }
            if ((sender as DataGrid).SelectedItem is Issue issue)
            {
                _aIssueViewModel.GetAIssue(issue.Id);
                _aIssueViewModel.CanAssignUsersList.Clear();
                _aIssueViewModel.CanRemoveUsersList.Clear();
                //TasksList.DataContext = _task;
            }
        }

        private async void DeleteIssue_Click(object sender, RoutedEventArgs e)
        {
            int result = await ConfirmtionDialogue();
            if (result == 1)
                _deleteIssueViewModel.DeleteIssue(_aIssueViewModel.AIssue.FirstOrDefault().Issue.Id);
        }

        private void AddIssueForm_Closed(object sender, object e)
        {
            CreateIssueForm.ClearFormData();
        }

        public async Task<int> ConfirmtionDialogue()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Delete Issue?";
            dialog.Content = "Once deleted, issue cannot be retrieved";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Close;
            var result = await dialog.ShowAsync();
            return (int)result;
        }

        public void NotificationMessage()
        {
            IssuePageNotification.Invoke(_deleteIssueViewModel.ResponseString);
        }

        public void ShowIssuePageNotification(string msg)
        {
            NoitificationBox.Show(msg, 3000);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IssuesList.Visibility = Visibility.Visible;
            IssueGridSplitter.Visibility = Visibility.Collapsed;
            IssueDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(IssuesList, 0);
            Grid.SetColumnSpan(IssuesList, 3);
            _itemSelected = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateIssueForm.GetFormData(CUser.LoggedInUser.Name, _issueViewModel.projectId);
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            IssueOfAProject.Visibility = Visibility.Visible;
            //TasksList.Visibility = Visible
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(IssuesList, 0);
            Grid.SetColumnSpan(IssuesList, 3);
            IssuesList.Visibility = Visibility.Visible;
            IssueGridSplitter.Visibility = Visibility.Collapsed;
            IssueDetailGrid.Visibility = Visibility.Collapsed;
        }

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    CreateIssueForm.ClearFormData();
        //    AddIssueForm.Visibility = Visibility.Collapsed;
        //}


        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;

            if (_windowWidth < 900)
            {
                IssueOfAProject.FrozenColumnCount = 1;
                NewIssueButton.Visibility = Visibility.Collapsed;
                _narrowLayout = true;
                CloseButton.Visibility = Visibility.Collapsed;
                if (_itemSelected)
                {
                    IssueDetailGrid.Visibility = Visibility.Collapsed;
                    IssuesList.Visibility = Visibility.Collapsed;
                    IssueDetailGrid.Visibility = Visibility.Visible;
                    IssueDetailsPage.Visibility = Visibility.Visible;
                    Grid.SetColumn(IssuesList, 2);
                    Grid.SetColumnSpan(IssuesList, 1);
                    Grid.SetColumn(IssueDetailGrid, 0);
                    Grid.SetColumnSpan(IssueDetailGrid, 3);
                    BackToList.Visibility = Visibility.Visible;
                }
            }
            else
            {
                IssueOfAProject.FrozenColumnCount = 2;
                _narrowLayout = false;
                CloseButton.Visibility = Visibility.Visible;
                if (_itemSelected)
                {
                    Grid.SetColumn(IssuesList, 0);
                    Grid.SetColumn(IssueGridSplitter, 1);
                    Grid.SetColumn(IssueDetailGrid, 2);
                    Grid.SetColumnSpan(IssuesList, 1);
                    Grid.SetColumnSpan(IssueGridSplitter, 1);
                    Grid.SetColumnSpan(IssueDetailGrid, 1);
                    IssuesList.Visibility = Visibility.Visible;
                    IssueGridSplitter.Visibility = Visibility.Visible;
                    IssueDetailGrid.Visibility = Visibility.Visible;
                    BackToList.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
