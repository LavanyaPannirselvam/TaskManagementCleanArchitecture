using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
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
using TaskManagementLibrary.Notifications;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class Issues : UserControl , IIssuePageUpdateNotification
    {
        private static bool _itemSelected; 
        private Issue _issue = new Issue();
        public IssuesViewModelBase _issueViewModel;
        public IssueDetails issueDetailsPage;
        public static event Action<string> IssuePageNotification;
        //private static Dictionary<int, AppWindow> _appWindows = new Dictionary<int, AppWindow>();
        private double _windowHeight;
        private double _windowWidth;
        private bool _narrowLayout;

        //public static readonly DependencyProperty UserProperty = DependencyProperty.Register("CUser", typeof(LoggedInUserBO), typeof(IssuesPage), new PropertyMetadata(null));

        //public LoggedInUserBO CUser
        //{
        //    get { return (LoggedInUserBO)GetValue(UserProperty); }
        //    set { SetValue(UserProperty, value); }
        //}

        public Issues()
        {
            this.InitializeComponent();
            _issueViewModel = PresenterService.GetInstance().Services.GetService<IssuesViewModelBase>();
            _issueViewModel.Notification = this;
        }

        private void NewIssueButton_Click(object sender, RoutedEventArgs e)
        {
            if(IssueDetailGrid.Visibility == Visibility.Visible)
            {
                IssuesList.Visibility = Visibility.Visible;
                IssueGridSplitter.Visibility = Visibility.Collapsed;
                IssueDetailGrid.Visibility = Visibility.Collapsed;
                Grid.SetColumn(IssuesList, 0);
                Grid.SetColumnSpan(IssuesList, 3);
                _itemSelected = false;
            }
            AddIssueForm.IsOpen = true;
            //double horizontalOffset = Window.Current.Bounds.Width / 2 - AddIssueForm.ActualWidth / 4 + 420;
            //double verticalOffset = Window.Current.Bounds.Height / 2 - AddIssueForm.ActualHeight / 2 - 300;
            //AddIssueForm.HorizontalOffset = horizontalOffset;
            //AddIssueForm.VerticalOffset = verticalOffset;
            ErrorMessage.Visibility = Visibility.Collapsed;
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
                issueDetailsPage = new IssueDetails(issue.Id);
                issueDetailsPage.DataContext = issue;
                IssuesList.DataContext = _issue;
                IssueOfAProject.SelectedIndex = -1;
                //TasksList.DataContext = _task;
            }
        }

        private async void DeleteIssue_Click(object sender, RoutedEventArgs e)
        {
            int result = await ConfirmtionDialogue();
            if (result == 1)
            {
                IssuesList.Visibility = Visibility.Visible;
                IssueGridSplitter.Visibility = Visibility.Collapsed;
                IssueDetailGrid.Visibility = Visibility.Collapsed;
                Grid.SetColumn(IssuesList, 0);
                Grid.SetColumnSpan(IssuesList, 3);
                _itemSelected = false;
                _issueViewModel.DeleteIssue(issueDetailsPage._issueViewModel.SelectedIssue.Id);
            }
        }

        //private void AddIssueForm_Closed(object sender, object e)
        //{
        //    CreateIssueForm.ClearFormData();
        //    ErrorMessage.Text = string.Empty;
        //}

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
            dialog.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            var result = await dialog.ShowAsync();
            return (int)result;
        }

        public void NotificationMessage()
        {
            IssuePageNotification.Invoke(_issueViewModel.ResponseString);
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
            ErrorMessage.Text = string.Empty;
            Issue pro = CreateIssueForm.GetFormData(CurrentUserClass.CurrentUser.Name, _issueViewModel.projectId);
            if (pro != null)
            {
                AddIssueForm.IsOpen = false;
                _issueViewModel.CreateIssue(pro);
            }
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            IssueOfAProject.Visibility = Visibility.Visible;
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(IssuesList, 0);
            Grid.SetColumnSpan(IssuesList, 3);
            IssuesList.Visibility = Visibility.Visible;
            IssueGridSplitter.Visibility = Visibility.Collapsed;
            IssueDetailGrid.Visibility = Visibility.Collapsed;
        }
      
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
                    IssueGridSplitter.Visibility = Visibility.Collapsed;
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
                NewIssueButton.Visibility = Visibility.Visible;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IssuePageNotification += ShowIssuePageNotification;
            UIUpdation.IssueCreated += UIUpdation_IssueCreated;
            UIUpdation.IssueDeleted += UIUpdation_IssueDeleted;
            this.FindName("IssuePage");
            this.UnloadObject(Projectpage);
            IssuesList.Visibility = Visibility.Visible;
            IssueGridSplitter.Visibility = Visibility.Collapsed;
            IssueDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(IssuesList, 0);
            Grid.SetColumnSpan(IssuesList, 3);
            _itemSelected = false;
        }

        private void UIUpdation_IssueDeleted(Issue issue)
        {
            var delete = _issueViewModel.IssuesList.Where(i => i.Id == issue.Id);
            _issueViewModel.IssuesList.Remove(delete.FirstOrDefault());
        }

        private void UIUpdation_IssueCreated(Issue obj)
        {
            _issueViewModel.IssuesList.Add(obj);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            IssuePageNotification -= ShowIssuePageNotification;
            UIUpdation.IssueCreated -= UIUpdation_IssueCreated;
            UIUpdation.IssueDeleted -= UIUpdation_IssueDeleted;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.UnloadObject(IssuePage);
            UIUpdation.OnBackNavigated();
        }

        private void ClosePopUpButton_Click(object sender, RoutedEventArgs e)
        {
            AddIssueForm.IsOpen = false;
            CreateIssueForm.ClearFormData();
        }
    }
}
