using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class TasksPage : UserControl, INotifyPropertyChanged, IDeleteTaskNotification
    {
        private static bool _itemSelected;
        private Tasks _task = new Tasks();
        public TasksViewModelBase _taskViewModel;
        public ATaskViewModelBase _aTaskViewModel;
        public DeleteTaskViewModelBase _deleteTaskViewModel;
        private double _windowWidth;
        private double _windowHeight;
        private bool _narrowLayout;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(CUser), typeof(LoggedInUserBO), typeof(TasksPage), new PropertyMetadata(null));
        public static event Action<string> TaskPageNotification;
        public LoggedInUserBO CUser
        {
            get { return (LoggedInUserBO)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TasksPage()
        {
            this.InitializeComponent();
            _taskViewModel = PresenterService.GetInstance().Services.GetService<TasksViewModelBase>();
            _aTaskViewModel = PresenterService.GetInstance().Services.GetService<ATaskViewModelBase>();
            _deleteTaskViewModel = PresenterService.GetInstance().Services.GetService<DeleteTaskViewModelBase>();
            _deleteTaskViewModel.Notification = this;
            TaskPageNotification += ShowTaskPageNotiifcation;
            //if(_aTaskViewModel.ATask.Count > 0)
            //{
            //    DataGridVisibility.Visibility = Visibility.Collapsed;
            //}
            //_taskViewModel.TasksList.Clear();
            //_taskViewModel.
        }

        private void TasksList_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
                e.Column.Header = "Task Id";
            if (e.Column.Header.ToString() == "ProjectId")
                e.Column.Header = "Project Id";
            if (e.Column.Header.ToString() == "CreateddBy")
                e.Column.Header = "Created by";
            if (e.Column.Header.ToString() == "StartDate")
                e.Column.Header = "Start Date";
            if (e.Column.Header.ToString() == "EndDate")
                e.Column.Header = "End Date";
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            _itemSelected = false;
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            TasksOfAProject.Visibility = Visibility.Visible;
            //TasksList.Visibility = Visible
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
        }

        //private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    double windowHeight = e.NewSize.Height;
        //    double windowWidth = e.NewSize.Width;

        //    if (windowHeight < 300 || windowWidth < 900)
        //    {
        //        _narrowLayout = true;
        //        CloseButton.Visibility = Visibility.Collapsed;

        //        if (_itemSelected)
        //        {

        //            TasksGridSplitter.Visibility = Visibility.Collapsed;
        //            TasksList.Visibility = Visibility.Collapsed;
        //            TasksDetailGrid.Visibility = Visibility.Visible;
        //            TaskDetailsPage.Visibility = Visibility.Visible;
        //            Grid.SetColumn(TasksList, 2);
        //            Grid.SetColumnSpan(TasksList, 1);
        //            Grid.SetColumn(TasksDetailGrid, 0);
        //            Grid.SetColumnSpan(TasksDetailGrid, 3);
        //            BackToList.Visibility = Visibility.Visible;
        //        }
        //    }
        //    else
        //    {
        //        _narrowLayout = false;
        //        CloseButton.Visibility = Visibility.Visible;

        //        if (_itemSelected)
        //        {
        //            Grid.SetColumn(TasksList, 0);
        //            Grid.SetColumn(TasksGridSplitter, 1);
        //            Grid.SetColumn(TasksDetailGrid, 2);
        //            Grid.SetColumnSpan(TasksList, 1);
        //            Grid.SetColumnSpan(TasksGridSplitter, 1);
        //            Grid.SetColumnSpan(TasksDetailGrid, 1);
        //            TasksList.Visibility = Visibility.Visible;
        //            TasksGridSplitter.Visibility = Visibility.Visible;
        //            TasksDetailGrid.Visibility = Visibility.Visible;
        //            BackToList.Visibility = Visibility.Collapsed;
        //        }

        //    }

       // }

        private void TasksOfAProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _itemSelected = true;
            if (_narrowLayout)
            {
                _narrowLayout = true;
                TasksList.Visibility = Visibility.Collapsed;
                TasksGridSplitter.Visibility = Visibility.Collapsed;
                TasksDetailGrid.Visibility = Visibility.Visible;
                Grid.SetColumn(TasksDetailGrid, 0);
                Grid.SetColumnSpan(TasksDetailGrid, 3);
                BackToList.Visibility = Visibility.Visible;
             
            }
            else
            {
                _narrowLayout = false;
                Grid.SetColumn(TasksList, 0);
                Grid.SetColumn(TasksGridSplitter, 1);
                Grid.SetColumn(TasksDetailGrid, 2);
                Grid.SetColumnSpan(TasksList, 1);
                Grid.SetColumnSpan(TasksGridSplitter, 1);
                Grid.SetColumnSpan(TasksDetailGrid, 1);
                TasksList.Visibility = Visibility.Visible;
                TasksGridSplitter.Visibility = Visibility.Visible;
                TasksDetailGrid.Visibility = Visibility.Visible;
                
            }
            if ((sender as DataGrid).SelectedItem is Tasks task)
            {
                _aTaskViewModel.GetATask(task.Id);
                _aTaskViewModel.CanAssignUsersList.Clear();
                _aTaskViewModel.CanRemoveUsersList.Clear();
                TasksList.DataContext = _task;
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskForm.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - AddTaskForm.ActualWidth / 4 + 300;
            double verticalOffset = Window.Current.Bounds.Height / 2 - AddTaskForm.ActualHeight / 2 - 300;
            AddTaskForm.HorizontalOffset = horizontalOffset;
            AddTaskForm.VerticalOffset = verticalOffset;
            AddTaskForm.IsOpen = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskForm.GetFormData(CUser.LoggedInUser.Name, _taskViewModel.projectId);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskForm.ClearFormData();
            AddTaskForm.Visibility = Visibility.Collapsed;
        }

        private void AddTaskForm_Closed(object sender, object e)
        {
            CreateTaskForm.ClearFormData();
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            int result = await ConfirmtionDialogue();
            if(result == 1)
                _deleteTaskViewModel.DeleteTask(_aTaskViewModel.ATask.FirstOrDefault().Tasks.Id);
        }

        public async Task<int> ConfirmtionDialogue()
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Delete Task?";
            dialog.Content = "Once deleted, task cannot be retrieved";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Close;
            var result = await dialog.ShowAsync();
            return (int)result;
        }

        public void NotificationMessage()
        {
            TaskPageNotification.Invoke(_deleteTaskViewModel.ResponseString);
        }

        public void ShowTaskPageNotiifcation(string msg)
        {
            NoitificationBox.Show(msg, 3000);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;
            if (_windowWidth < 900)
            {
                TasksOfAProject.FrozenColumnCount = 1;
                NewTaskButton.Visibility = Visibility.Collapsed;
                _narrowLayout = true;
                CloseButton.Visibility = Visibility.Collapsed;
                if (_itemSelected)
                {
                    TasksGridSplitter.Visibility = Visibility.Collapsed;
                    TasksList.Visibility = Visibility.Collapsed;
                    TasksDetailGrid.Visibility = Visibility.Visible;
                    TaskDetailsPage.Visibility = Visibility.Visible;
                    Grid.SetColumn(TasksList, 2);
                    Grid.SetColumnSpan(TasksList, 1);
                    Grid.SetColumn(TasksDetailGrid, 0);
                    Grid.SetColumnSpan(TasksDetailGrid, 3);
                    BackToList.Visibility = Visibility.Visible;
                }
            }
            else
            {
                TasksOfAProject.FrozenColumnCount = 2;
                _narrowLayout = false;
                CloseButton.Visibility = Visibility.Visible;
                if (_itemSelected)
                {
                    Grid.SetColumn(TasksList, 0);
                    Grid.SetColumn(TasksGridSplitter, 1);
                    Grid.SetColumn(TasksDetailGrid, 2);
                    Grid.SetColumnSpan(TasksList, 1);
                    Grid.SetColumnSpan(TasksGridSplitter, 1);
                    Grid.SetColumnSpan(TasksDetailGrid, 1);
                    TasksList.Visibility = Visibility.Visible;
                    TasksGridSplitter.Visibility = Visibility.Visible;
                    TasksDetailGrid.Visibility = Visibility.Visible;
                    BackToList.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}

