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
using TaskManagementLibrary.Notifications;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class TasksUC : UserControl, INotifyPropertyChanged, ITaskUpdation
    {
        private static bool _itemSelected;
        private Tasks _task = new Tasks();
        public TasksPageViewModelBase _taskViewModel;
        private double _windowWidth;
        private double _windowHeight;
        TaskDetails taskDetailsPage;
        private bool _narrowLayout;
        public static event Action<string> TaskPageNotification;
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TasksUC()
        {
            this.InitializeComponent();
            _taskViewModel = PresenterService.GetInstance().Services.GetService<TasksPageViewModelBase>();
            _taskViewModel.updation = this;
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
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
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
                taskDetailsPage = new TaskDetails(task.Id);
                taskDetailsPage.DataContext = task;
                TasksList.DataContext = _task;
                TasksOfAProject.SelectedIndex = -1;
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if(TasksDetailGrid.Visibility == Visibility.Visible)
            {
                TasksList.Visibility = Visibility.Visible;
                TasksGridSplitter.Visibility = Visibility.Collapsed;
                TasksDetailGrid.Visibility = Visibility.Collapsed;
                Grid.SetColumn(TasksList, 0);
                Grid.SetColumnSpan(TasksList, 3);
                _itemSelected = false;
            }
            AddTaskForm.IsOpen = true;
            //double horizontalOffset = Window.Current.Bounds.Width / 2 - AddTaskForm.ActualWidth / 4 + 420;
            //double verticalOffset = Window.Current.Bounds.Height / 2 - AddTaskForm.ActualHeight / 2 - 300;
            //AddTaskForm.HorizontalOffset = horizontalOffset;
            //AddTaskForm.VerticalOffset = verticalOffset;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = string.Empty;
            Tasks pro = CreateTaskForm.GetFormData(CurrentUserClass.CurrentUser.Name, _taskViewModel.projectId);
            if (pro != null)
            {
                AddTaskForm.IsOpen = false;
                CreateTaskForm.ClearFormData();
                _taskViewModel.CreateNewTask(pro);
            }
        }

        private void AddTaskForm_Closed(object sender, object e)
        {
            CreateTaskForm.ClearFormData();
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            int result = await ConfirmtionDialogue();
            //TasksDetailGrid.Visibility = Visibility.Collapsed;
            if (result == 1)
            {
                //TasksList.Visibility = Visibility.Visible;
                //TasksGridSplitter.Visibility = Visibility.Collapsed;
                //TasksDetailGrid.Visibility = Visibility.Collapsed;
                //Grid.SetColumn(TasksList, 0);
                //Grid.SetColumnSpan(TasksList, 3);
                _itemSelected = false;//TODO
                _taskViewModel.DeleteTask(taskDetailsPage._taskDetailsViewModel.SelectedTask.Id);
            }
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
            dialog.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            var result = await dialog.ShowAsync();
            return (int)result;
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
            else if(_windowWidth >= 900 && _windowWidth <= 1200)
            {
                TasksOfAProject.FrozenColumnCount = 2;
                _narrowLayout = false;
                NewTaskButton.Visibility = Visibility.Visible;
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
                NewTaskButton.Visibility = Visibility.Visible;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIUpdation.TaskCreated += UpdateNewTask;
            UIUpdation.TaskDeleted += UpdateDeleteTask;
            UIUpdation.TaskUpdated += UIUpdation_TaskUpdated;
            TaskPageNotification += ShowTaskPageNotiifcation;
            this.FindName("TaskPage");
            this.UnloadObject(Projectpage);
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
            if (_taskViewModel.TasksList.Count >= 20)
            {
                GridRow.Height = new GridLength(750, GridUnitType.Pixel);
            }
            _itemSelected = false;
        }

        private void UIUpdation_TaskUpdated(Tasks obj)
        {
            var issue = _taskViewModel.TasksList.Where(i => i.Id == obj.Id).FirstOrDefault();
            var index = _taskViewModel.TasksList.IndexOf(issue);
            _taskViewModel.TasksList.Remove(issue);
            _taskViewModel.TasksList.Insert(index, obj);
            taskDetailsPage._taskDetailsViewModel.GetATask(obj.Id);
        }

        private void UpdateDeleteTask(Tasks tasks)
        {
            var delete = _taskViewModel.TasksList.Where(task => task.Id == tasks.Id);
            _taskViewModel.TasksList.Remove(delete.FirstOrDefault());
            TasksList.Visibility = Visibility.Visible;
            TasksGridSplitter.Visibility = Visibility.Collapsed;
            TasksDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TasksList, 0);
            Grid.SetColumnSpan(TasksList, 3);
        }

        private void UpdateNewTask(Tasks tasks)
        {
            _taskViewModel.TasksList.Add(tasks);
            if (_taskViewModel.TasksList.Count >= 20)
            {
                GridRow.Height = new GridLength(750, GridUnitType.Pixel);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            TaskPageNotification -= ShowTaskPageNotiifcation;
            UIUpdation.TaskCreated -= UpdateNewTask;
            UIUpdation .TaskDeleted -= UpdateDeleteTask;
            UIUpdation.TaskUpdated -= UIUpdation_TaskUpdated;
        }

        public void TaskUpdationNotification()
        {
            TaskPageNotification.Invoke(_taskViewModel.ResponseString);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.UnloadObject(TaskPage);
            UIUpdation.OnBackNavigated();
        }

        private void ClosePopUpButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskForm.IsOpen = false;
            CreateTaskForm.ClearFormData();
        }

        //private async Task PopoutButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AppWindow appWindow = await AppWindow.TryCreateAsync();
        //    Frame appWindowContentFrame = new Frame();
        //    //IssueDetailsPage issueDetailsPage = new IssueDetailsPage();
        //    appWindowContentFrame.Navigate(typeof(TaskDetailsPage));
        //    ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
        //    await appWindow.TryShowAsync();
        //}

        //private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        //{
        //    _taskViewModel.GetTasks(_taskViewModel.TasksList.First().ProjectId,20,_taskViewModel.TasksList.Count);
        //}
    }
}

