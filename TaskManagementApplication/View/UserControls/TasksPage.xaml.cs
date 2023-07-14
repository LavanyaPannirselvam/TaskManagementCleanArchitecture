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

        //private void BackToList_Click(object sender, RoutedEventArgs e)
        //{
        //    TasksOfAProject.Visibility = Visibility.Visible;
        //    //TasksList.Visibility = Visible
        //    BackToList.Visibility = Visibility.Collapsed;
        //    _itemSelected = false;
        //    Grid.SetColumn(TasksList, 0);
        //    Grid.SetColumnSpan(TasksList, 3);
        //    TasksList.Visibility = Visibility.Visible;
        //    TasksGridSplitter.Visibility = Visibility.Collapsed;
        //    TasksDetailGrid.Visibility = Visibility.Collapsed;
        //}


        private void TasksOfAProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _itemSelected = true;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumn(TasksGridSplitter, 1);
            Grid.SetColumn(TasksDetailGrid, 2);
            Grid.SetColumnSpan(TasksList, 1);
            Grid.SetColumnSpan(TasksGridSplitter, 1);
            Grid.SetColumnSpan(TasksDetailGrid, 1);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Visible;
            TasksDetailGrid.Visibility = Visibility.Visible;
            if ((sender as DataGrid).SelectedItem is Tasks task)
            {
                _aTaskViewModel.GetATask(task.Id);
                _aTaskViewModel.CanAssignUsersList.Clear();
                _aTaskViewModel.CanRemoveUsersList.Clear();
                //TasksList.DataContext = _task;
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            double horizontalOffset = Window.Current.Bounds.Width / 2; // - AddTaskForm.ActualWidth / 2;
            double verticalOffset = Window.Current.Bounds.Height / 2; // - AddTaskForm.ActualHeight / 2;
            AddTaskForm.HorizontalOffset = horizontalOffset;
            AddTaskForm.VerticalOffset = verticalOffset;
            AddTaskForm.IsOpen = true;
            AddTaskForm.Visibility = Visibility.Visible;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskForm.GetFormData(CUser.LoggedInUser.Name, _aTaskViewModel.ATask.FirstOrDefault().Tasks.ProjectId);
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

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Delete Task?";
            dialog.Content = "Once deleted, task cannot be retrieved";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Close;
            //dialog.Content = new ContentDialogContent();

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
    }


}

