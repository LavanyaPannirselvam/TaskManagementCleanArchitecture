﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
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
    public sealed partial class Projects : UserControl , INotifyPropertyChanged , IProjectPageUpdate
    {
        public ProjectsViewModelBase _projectsPageViewModel;
        private int projectId = 0;
        public Issues issuePage = new Issues();
        public TasksUC tasksPage = new TasksUC();
        public static event Action<string> Notification;
        private double _windowHeight;
        private double _windowWidth;

        public Projects()
        {
            this.InitializeComponent();
            _projectsPageViewModel = PresenterService.GetInstance().Services.GetService<ProjectsViewModelBase>();
            _projectsPageViewModel.projectPageUpdate = this;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProjectsList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id")
                e.Column.Header = "Project Id";
            if (e.Column.Header.ToString() == "CreateddBy")
                e.Column.Header = "Created by";
            if (e.Column.Header.ToString() == "StartDate")
                e.Column.Header = "Start Date";
            if (e.Column.Header.ToString() == "EndDate")
                e.Column.Header = "End Date";
        }

        private void ProjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as DataGrid).SelectedItem is Project project)
            {
                projectId = project.Id;
                tasksPage._taskViewModel.projectId = projectId;
                issuePage._issueViewModel.projectId = projectId;
                tasksPage._taskViewModel.TasksList.Clear();
                tasksPage._taskViewModel.GetTasks(projectId);
                issuePage._issueViewModel.IssuesList.Clear();
                issuePage._issueViewModel.GetIssues(projectId);
                ProjectsList.SelectedIndex = -1;
                _projectsPageViewModel.ProjectsList.Clear();
                this.FindName("taskofaproject");
                this.UnloadObject(ProjectPageGrid);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Project pro = CreateProjectForm.GetFormData(CurrentUserClass.CurrentUser.Name);
            if (pro != null)
            {
                AddProjectForm.IsOpen = false;
                CreateProjectForm.ClearFormData();
                _projectsPageViewModel.CreateProject(pro);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.FindName("NotificationBox");
            _projectsPageViewModel.GetProjectsList(CurrentUserClass.CurrentUser.Name, CurrentUserClass.CurrentUser.Email);
            if (_projectsPageViewModel.ProjectsList.Count >= 20)
            {
                GridRow.Height = new GridLength(750, GridUnitType.Pixel);
            }
            Notification += ShowNotification;
            UIUpdation.ProjectCreated += UpdateNewProject;
        }

        private void UpdateNewProject(Project project)
        {
            if (!_projectsPageViewModel.ProjectsList.Contains<Project>(project))
            {
                _projectsPageViewModel.ProjectsList.Add(project);
                if (ProjectPageGrid != null && ProjectPageGrid.IsLoaded)
                {
                    if (_projectsPageViewModel.ProjectsList.Count >= 20)
                    {
                        ProjectsList.Height = 750;
                    }
                }
            }
        }

        private void ShowNotification(string obj)
        {
            if (NotificationBox != null)
            {
                NotificationBox.Show(obj, 3000);
            }
            else
            {}
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            //double horizontalOffset = Window.Current.Bounds.Width / 2 - AddProjectForm.ActualWidth / 2 + 100;
            //double verticalOffset = Window.Current.Bounds.Height / 2 - AddProjectForm.ActualHeight / 2 - 300;
            //AddProjectForm.HorizontalOffset = horizontalOffset;
            //AddProjectForm.VerticalOffset = verticalOffset;
            AddProjectForm.IsOpen = true;
            AddProjectForm.Visibility = Visibility.Visible;
            ErrorMessage.Text = string.Empty;
        }

        private void AddProjectForm_Closed(object sender, object e)
        {
            CreateProjectForm.ClearFormData();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;
            if (ProjectPageGrid != null && ProjectPageGrid.IsLoaded)
            {
                if (_windowWidth < 900)
                {
                    NewProjectButton.Visibility = Visibility.Collapsed;
                    ProjectsList.FrozenColumnCount = 1;
                    AddProjectForm.IsOpen = false;
                }
                else
                {
                    NewProjectButton.Visibility = Visibility.Visible;
                    ProjectsList.FrozenColumnCount = 2;
                }
            }
            //this.UnloadObject(ProjectPageGrid);
        }

        public void ProjectUpdateNotification()
        {
            Notification.Invoke(_projectsPageViewModel.ResponseString);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
            UIUpdation.ProjectCreated -= UpdateNewProject;
            _projectsPageViewModel.ProjectsList.Clear();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectForm.IsOpen = false;
            CreateProjectForm.ClearFormData() ;
        }
    }
}
