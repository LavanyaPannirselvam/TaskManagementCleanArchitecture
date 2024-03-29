﻿using Microsoft.Extensions.DependencyInjection;
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
using TaskManagementCleanArchitecture.View;
using TaskManagementCleanArchitecture.View.UserControls;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        public event PropertyChangedEventHandler PropertyChanged;
        private AppWindow settingPage;
        public FirstPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public LoggedInUserBO CurrentUser = CurrentUserClass.CurrentUser;

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
            if (e.Parameter is FirstPage pg)
            {
                AdminTabVisibility = pg.AdminTabVisibility;
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

        private async Task PopoutButton_Click()
        {
            if (settingPage == null)
            {
                settingPage = await AppWindow.TryCreateAsync();
                Frame appWindowContentFrame = new Frame();
                appWindowContentFrame.Navigate(typeof(SettingsPage));
                ElementCompositionPreview.SetAppWindowContent(settingPage, appWindowContentFrame);
                appWindowContentFrame.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
                ThemeSwitch.AddUIRootElement(appWindowContentFrame);
                settingPage.Closed += SettingPage_Closed;
                await settingPage.TryShowAsync();
            }
            else
            {
                await settingPage.TryShowAsync();
            }
        }

        private void SettingPage_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            settingPage = null;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var result = MainPageNV.IsPaneOpen;
            MainPageNV.IsPaneOpen = result;
            var theme = ((ToggleSwitch)sender).IsOn ? 0 : 1;
            ThemeChange_Tapped(theme);
        }

        private void ThemeChange_Tapped(int value)
        {
            if (value == 0)
            {
                ThemeSwitch.CurrentTheme = ElementTheme.Dark;
                ThemeSwitch.UIUpdation_ThemeSwitched();
                AccentChange.UpdateAccentBasedOnTheme(ThemeSwitch.CurrentTheme);
            }
            else
            {
                ThemeSwitch.CurrentTheme = ElementTheme.Light;
                ThemeSwitch.UIUpdation_ThemeSwitched();
                AccentChange.UpdateAccentBasedOnTheme(ThemeSwitch.CurrentTheme);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageNV.SelectedItem = ProjectsTab;
            ThemeChange.Toggled += ToggleSwitch_Toggled;
            ThemeChange.IsOn = ThemeSwitch.CurrentTheme == ElementTheme.Dark;
        }

        private void Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIUpdation.OnUserLogout();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ThemeChange.Toggled -= ToggleSwitch_Toggled;
        }

        private async void SettingsTab_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await PopoutButton_Click();
        }
    }
}
