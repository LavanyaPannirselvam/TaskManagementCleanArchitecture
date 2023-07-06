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
using TaskManagementCleanArchitecture.View.UserControls;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static TaskManagementLibrary.Models.User;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskManagementCleanArchitecture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstPage : Page, INotifyPropertyChanged
    {
        //public FirstPageViewModelBase _firstPageViewModel;
        //private int projectId = 0;

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(CurrentUser), typeof(LoggedInUserBO), typeof(FirstPage), new PropertyMetadata(null));

        public LoggedInUserBO CurrentUser
        {
            get { return (LoggedInUserBO)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        //private LoggedInUserBO _currentUser;
        //public LoggedInUserBO CUser
        //{
        //    get { return _currentUser; }
        //    set
        //    {
        //        _currentUser = value;
        //        NotifyPropertyChanged(nameof(CUser));
        //    }
        //}

        private String _headerTitle = "Projects";
        public String HeaderTitle
        {
            get { return _headerTitle; }
            set
            {
                _headerTitle = value;
                NotifyPropertyChanged(nameof(HeaderTitle));
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;


        public FirstPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            //_firstPageViewModel = PresenterService.GetInstance().Services.GetService<FirstPageViewModelBase>();
            //_firstPageViewModel.GetProjectsList();
        }

        //private void ProjectListDetailsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Project project = (sender as ListDetailsView).SelectedItem as Project;
        //    projectId = project.Id;
        //    _firstPageViewModel.GetUsersList(projectId);
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is FirstPage pg && pg.CurrentUser!=null)
            {
                CurrentUser = pg.CurrentUser;
            }
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TasksPage.OnLoadingSuccess += NavigateToTasksPage;

        //}

        //private void NavigateToTasksPage(ObservableCollection<TaskManagementLibrary.Models.User> users)
        //{
        //    TasksPage tasksPage = new TasksPage();
        //    //tasksPage.Users = users;
        //    //contentFrame.Navigate(typeof(TasksPage), users);
        //    NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate2"]).LoadContent();

        //}

        private void FirstPageNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationContentControl.DataContext = this;
            if (args.SelectedItem == ProjectsTab)
            {
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate1"]).LoadContent();
                MainPageNV.AlwaysShowHeader = true;
            }
            else if (args.SelectedItem == TasksTab)
            {
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate2"]).LoadContent();
                MainPageNV.AlwaysShowHeader = true;
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // Save theme choice to LocalSettings. 
            // ApplicationTheme enum values: 0 = Light, 1 = Dark
            ApplicationData.Current.LocalSettings.Values["themeSetting"] =
                                                             ((ToggleSwitch)sender).IsOn ? 0 : 1;
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = App.Current.RequestedTheme == ApplicationTheme.Light;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageNV.SelectedItem = ProjectsTab;
        }
    }
}
