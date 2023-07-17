using Microsoft.Extensions.DependencyInjection;
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
    public sealed partial class ProjectsPage : UserControl , INotifyPropertyChanged
    {
        public ProjectsViewModelBase _projectsPageViewModel;
        public TasksViewModelBase _tasksViewModel;
        public IssuesViewModelBase _issuesViewModel;
        private int projectId = 0;
        private CreateProjectViewModelBase _createProjectViewModel;
        private double _windowHeight;
        private double _windowWidth;

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("CUser", typeof(LoggedInUserBO), typeof(ProjectsPage), new PropertyMetadata(null));

        public LoggedInUserBO CUser
        {
            get { return (LoggedInUserBO)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public ProjectsPage()
        {
            this.InitializeComponent();
            _projectsPageViewModel = PresenterService.GetInstance().Services.GetService<ProjectsViewModelBase>();
            _tasksViewModel = PresenterService.GetInstance().Services.GetService<TasksViewModelBase>();
            _issuesViewModel = PresenterService.GetInstance().Services.GetService<IssuesViewModelBase>();
            // _usersViewModel.LoadUsers = this;
            //if (_projectsPageViewModel.ProjectsList.Count == 0)
            //{
            //    _projectsPageViewModel.GetProjectsList();
            //}
            //Projects = _projectsPageViewModel.ProjectsList;
            //_projectsPageViewModel.ProjectsList.Clear();
            //_projectsPageViewModel.GetProjectsList();
            _createProjectViewModel = PresenterService.GetInstance().Services.GetService<CreateProjectViewModelBase>();

        }
        
        private ObservableCollection<Project> _projects = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                if(_projects == null)
                    _projects = value;
                NotifyPropertyChanged(nameof(Projects));
            }
        }

        
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
                _tasksViewModel.projectId = projectId;
                _issuesViewModel.projectId = projectId;
                _tasksViewModel.TasksList.Clear();
                _tasksViewModel.GetTasks(projectId);
                _issuesViewModel.IssuesList.Clear();
                _issuesViewModel.GetIssues(projectId);
                ProjectsList.Visibility = Visibility.Collapsed;
                TopOptions.Visibility = Visibility.Collapsed;
                taskofaproject.Visibility = Visibility.Visible;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = string.Empty;
            Project pro = CreateProjectForm.GetFormData(CUser.LoggedInUser.Name);
            if (pro.Name == string.Empty)
            {
                ErrorMessage.Text = "Fill all data";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            if (pro.StartDate < DateTime.Now)
            {
                ErrorMessage.Text = "Start date should not be yesterday";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            if (pro.EndDate < pro.StartDate)
            {
                ErrorMessage.Text = "End date should be greater than or equal to start date";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else _createProjectViewModel.CreateProject(pro);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectForm.ClearFormData();
            AddProjectForm.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _projectsPageViewModel.ProjectsList.Clear();
            _projectsPageViewModel.GetProjectsList(CUser.LoggedInUser.Name,CUser.LoggedInUser.Email);
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectForm.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - AddProjectForm.ActualWidth / 4 + 200;
            double verticalOffset = Window.Current.Bounds.Height / 2 - AddProjectForm.ActualHeight / 2 - 300;
            AddProjectForm.HorizontalOffset = horizontalOffset;
            AddProjectForm.VerticalOffset = verticalOffset;
            AddProjectForm.IsOpen = true;
            //CreateProjectForm.Visibility = Visibility.Visible;
        }

        private void AddProjectForm_Closed(object sender, object e)
        {
            CreateProjectForm.ClearFormData();
            //AddProjectForm.Visibility = Visibility.Collapsed;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _windowHeight = e.NewSize.Height;
            _windowWidth = e.NewSize.Width;

            if (_windowWidth < 900)
            {
                ProjectsList.FrozenColumnCount = 1;

            }
            else
                ProjectsList.FrozenColumnCount = 2;
        }
    }
}
