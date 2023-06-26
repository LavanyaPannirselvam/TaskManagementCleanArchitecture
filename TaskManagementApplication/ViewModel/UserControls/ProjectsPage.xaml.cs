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
        public FirstPageViewModelBase _firstPageViewModel;
        public TaskViewModelBase _tasksViewModel;
        private int projectId = 0;
        

        public ProjectsPage()
        {
            this.InitializeComponent();
            _firstPageViewModel = PresenterService.GetInstance().Services.GetService<FirstPageViewModelBase>();
            _tasksViewModel = PresenterService.GetInstance().Services.GetService<TaskViewModelBase>();
            // _usersViewModel.LoadUsers = this;
            if (_firstPageViewModel.ProjectsList.Count == 0)
            {
                _firstPageViewModel.GetProjectsList();
            }
            Projects = _firstPageViewModel.ProjectsList;
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

        private void ProjectListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project project = (sender as ListView).SelectedItem as Project;
            projectId = project.Id;
            _tasksViewModel.GetTasksList(projectId);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
