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
using TaskManagementLibrary.Notifications;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Input;
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

        //public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(CurrentUser), typeof(LoggedInUserBO), typeof(FirstPage), new PropertyMetadata(null));
        //public static readonly DependencyProperty SelectedUserControlProperty = DependencyProperty.Register(nameof(SelectedUserControl),typeof(UserControl),typeof(FirstPage),new PropertyMetadata(null));
        public event PropertyChangedEventHandler PropertyChanged;
       // public static event Action LogoutEvent;
        
        public FirstPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public LoggedInUserBO CurrentUser = CurrentUserClass.CurrentUser;
       
       
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

        private Visibility _adminTabVisibility = Visibility.Collapsed;
        public Visibility AdminTabVisibility
        {
            get { return _adminTabVisibility; }
            set
            {
                _adminTabVisibility = value;
                NotifyPropertyChanged(nameof(AdminTabVisibility));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Navigation navigation = new Navigation();
            if (e.Parameter is FirstPage pg)
            {
                //CurrentUser = pg.CurrentUser;
                AdminTabVisibility = pg.AdminTabVisibility;
                //navigation.CurrentUser = pg.CurrentUser;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            else if(args.SelectedItem == Admintab)
            {
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate3"]).LoadContent();
                MainPageNV.AlwaysShowHeader = true;
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // Save theme choice to LocalSettings. 
            // ApplicationTheme enum values: 0 = Light, 1 = Dark
            var theme = ((ToggleSwitch)sender).IsOn ? 0 : 1;
            ThemeChange_Tapped(theme);
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = true;
        }

        private async void ThemeChange_Tapped(int value)
        {
            if (value == 0)
            {
                await SwitchTheme.ChangeTheme(ElementTheme.Dark);
                SwitchTheme.CurrentTheme = ElementTheme.Dark;
            }
            else
            {
                await SwitchTheme.ChangeTheme(ElementTheme.Light);
                SwitchTheme.CurrentTheme = ElementTheme.Light;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageNV.SelectedItem = ProjectsTab;
        }

        private void Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIUpdation.OnUserLogout();
        }

        //private void MainPageNV_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        //{
        //    NavigationContentControl.DataContext = this;
        //    if (args.SelectedItem == ProjectsTab)
        //    {
        //        NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate1"]).LoadContent();
        //        ProjectsPage projectsPage = new ProjectsPage();
        //        SelectedUserControl = projectsPage;
        //        MainPageNV.AlwaysShowHeader = true;
        //    }
        //    else if (args.SelectedItem == TasksTab)
        //    {
        //        NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate2"]).LoadContent();
        //        TasksPage tasksPage = new TasksPage();
        //        SelectedUserControl = tasksPage;
        //        MainPageNV.AlwaysShowHeader = true;
        //    }
        //}
    }
}
