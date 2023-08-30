using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementCleanArchitecture.ViewModel.TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
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
    public sealed partial class CreatedTasks : UserControl
    {
        CreatedTasksPageViewModelBase _createdTask;
        private static bool _itemSelected;
        private bool _narrowLayout;
        private TaskDetails taskDetailsPage;
        private double _windowWidth;
        private double _windowHeight;
        private Tasks _task = new Tasks();
        private Style myStyle;

        public CreatedTasks()
        {
            this.InitializeComponent();
            _createdTask = PresenterService.GetInstance().Services.GetService<CreatedTasksPageViewModelBase>();
            taskDetailsPage = new TaskDetails();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.TaskUpdated += UIUpdation_TaskUpdated;
            UIUpdation.TaskDeleted += UIUpdation_TaskDeleted;
            _createdTask.TasksList.Clear();
            _createdTask.GetTasks(CurrentUserClass.CurrentUser.Name, CurrentUserClass.CurrentUser.Email);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            UIUpdation.TaskDeleted += UIUpdation_TaskDeleted;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            if (_createdTask.TasksList.Count >= 20)
            {
                GridRow.Height = new GridLength(720, GridUnitType.Pixel);
            }
            _itemSelected = false;
        }

        private void UIUpdation_TaskUpdated(Tasks obj)
        {
            var issue = _createdTask.TasksList.Where(i => i.Id == obj.Id).FirstOrDefault();
            var index = _createdTask.TasksList.IndexOf(issue);
            _createdTask.TasksList.Remove(issue);
            _createdTask.TasksList.Insert(index, obj);
            taskDetailsPage._taskDetailsViewModel.GetATask(obj.Id);
        }

        private void UIUpdation_TaskDeleted(Tasks obj)
        {
            var delete = _createdTask.TasksList.Where(task => task.Id == obj.Id);
            _createdTask.TasksList.Remove(delete.FirstOrDefault());
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
        }

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
                taskDetailsPage._taskDetailsViewModel.GetATask(task.Id);
                taskDetailsPage.DataContext = task;
                TasksList.DataContext = _task;
                TasksOfAProject.SelectedIndex = -1;
            }
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            TasksOfAProject.Visibility = Visibility.Visible;
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
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

        private void TasksList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            int result = await ConfirmtionDialogue();
            if (result == 1)
            {
               
                _itemSelected = false;
                _createdTask.DeleteTask(taskDetailsPage._taskDetailsViewModel.SelectedTask.Id);
            }
        }

        public async Task<int> ConfirmtionDialogue()
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            // dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Delete Task?";
            dialog.Content = "Once deleted, task cannot be retrieved";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            myStyle = (Style)Application.Current.Resources["AccentButtonStyleCustom"];
            dialog.CloseButtonStyle = myStyle;
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            var result = await dialog.ShowAsync();
            return (int)result;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;
            if (_windowWidth < 900)
            {
                TasksOfAProject.FrozenColumnCount = 1;
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
            else if (_windowWidth >= 900 && _windowWidth <= 1200)
            {
                TasksOfAProject.FrozenColumnCount = 2;
                _narrowLayout = false;
                CloseButton.Visibility = Visibility.Visible;
                SplitterColumn.MaxWidth = 400;
                SplitterColumn.MinWidth = 300;
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
            else
            {
                TasksOfAProject.FrozenColumnCount = 2;
                _narrowLayout = false;
                CloseButton.Visibility = Visibility.Visible;
                SplitterColumn.MaxWidth = 600;
                SplitterColumn.MinWidth = 500;
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _createdTask.TasksList.Clear();
            UIUpdation.TaskDeleted -= UIUpdation_TaskDeleted;
            UIUpdation.TaskUpdated -= UIUpdation_TaskUpdated;
        }

    }
}
