using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using static TaskManagementCleanArchitecture.View.UserControls.AssignedTasks;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class AssignedTasks : UserControl
    {
        AssignedTasksPageViewModelBase _assignedTasksPageViewModelBase;
        private static bool _itemSelected;
        private bool _narrowLayout;
        private TaskDetails taskDetailsPage; 
        private double _windowWidth;
        private double _windowHeight;
        private Tasks _task = new Tasks();

        public AssignedTasks()
        {
            this.InitializeComponent();
            _assignedTasksPageViewModelBase = PresenterService.GetInstance().Services.GetService<AssignedTasksPageViewModelBase>();
            taskDetailsPage = new TaskDetails();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.TaskUpdated += UIUpdation_TaskUpdated;
            _assignedTasksPageViewModelBase.GetTasks(CurrentUserClass.CurrentUser.Email);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3); 
            if (_assignedTasksPageViewModelBase.TasksList.Count >= 20)
            {
                GridRow.Height = new GridLength(700, GridUnitType.Pixel);
            }
            _itemSelected = false;
        }

        private void UIUpdation_TaskUpdated(Tasks obj)
        {
            var issue = _assignedTasksPageViewModelBase.TasksList.Where(i => i.Id == obj.Id).FirstOrDefault();
            var index = _assignedTasksPageViewModelBase.TasksList.IndexOf(issue);
            _assignedTasksPageViewModelBase.TasksList.Remove(issue);
            _assignedTasksPageViewModelBase.TasksList.Insert(index, obj);
            taskDetailsPage._taskDetailsViewModel.GetATask(obj.Id);
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
            _assignedTasksPageViewModelBase.TasksList.Clear();
            UIUpdation.TaskUpdated -= UIUpdation_TaskUpdated;
        }
    }
}
