using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class UserManagement : UserControl, IDeleteUserPageUpdateNotification , IUpdateSearchedUser
    {
        ObservableCollection<User> _matchingUsers;
        UserViewModelBase _viewModel;
        public static event Action<string> Notification;
        public static event Action<ObservableCollection<User>> UpdateSearchedUsers;
        AppWindow appWindow;
        Frame appWindowContentFrame;
        public UserManagement()
        {
            this.InitializeComponent();
            _viewModel = PresenterService.GetInstance().Services.GetService<UserViewModelBase>();
            _viewModel.deleteUserPageUpdateNotification = this;
            _viewModel.updateSearchedUser = this;
            _matchingUsers = new ObservableCollection<User>();
        }

        public void NotificationMessage()
        {
            Notification.Invoke(_viewModel.ResponseString);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.UsersList.Count == 0)
            {
                _viewModel.GetAllUsersList(9, 0);
            }
            Notification += ShowNotification;
            UIUpdation.NewUserCreated += NewUserCreated;
            UIUpdation.UserDeleted += UIUpdation_UserDeleted;
            UIUpdation.UserSelectedToDelete += UIUpdation_UserSelectedToDelete;
            UIUpdation.UserLogout += UIUpdation_UserLogout;
            UpdateSearchedUsers += UserManagement_UpdateSearchedUsers;
        }

        private void UserManagement_UpdateSearchedUsers(ObservableCollection<User> obj)
        {
            _matchingUsers.Clear();
            _matchingUsers = obj;
            if (_matchingUsers.Count == 0)
            {
                UsersGridView.Visibility = Visibility.Collapsed;
                NoUsersText.Visibility = Visibility.Visible;
            }
            else
            {
                UsersGridView.ItemsSource = _matchingUsers;
                UsersGridView.Visibility = Visibility.Visible;
                NoUsersText.Visibility = Visibility.Collapsed;
            }
        }

        private void UIUpdation_UserLogout()
        {
            _viewModel.UsersList.Clear();
        }

        private void UIUpdation_UserSelectedToDelete(string obj)
        {
            _viewModel.DeleteUser(obj);
        }

        private void UIUpdation_UserDeleted(User obj)
        {
            _viewModel.UsersList.Remove(_viewModel.UsersList.Where(u => u.Email == obj.Email).FirstOrDefault());
        }

        private void NewUserCreated(User obj)
        {
            _viewModel.UsersList.Add(obj);
        }

        private void ShowNotification(string msg)
        {
            NotificationControl.Show(msg, 3000);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
            UIUpdation.NewUserCreated -= NewUserCreated;
            UIUpdation.UserDeleted -= UIUpdation_UserDeleted;
        }

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            appWindow = await AppWindow.TryCreateAsync();
            appWindowContentFrame = new Frame();
            var newUserPage = new CreateNewUserPage();
            appWindowContentFrame.Navigate(newUserPage.GetType());
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            ThemeSwitch.AddUIRootElement(appWindowContentFrame);
            appWindowContentFrame.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            await appWindow.TryShowAsync();
            appWindow.Closed += appWindowClosed;
        }

        private void appWindowClosed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            appWindowContentFrame.Content = null;
            appWindow = null;
        }

        private void FindUsersBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var text = sender.Text;
                if (text.Length > 0)
                { 
                    _viewModel.GetMatchingUsers(text);
                }
                else
                {
                    UsersGridView.Visibility = Visibility.Visible;
                    NoUsersText.Visibility = Visibility.Collapsed;
                    UsersGridView.ItemsSource = _viewModel.UsersList;
                }
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            _viewModel.GetAllUsersList(12, _viewModel.UsersList.Count);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 950 )
            {
                GridViewScroller.Height = 670;
            }
            else if (e.NewSize.Width >= 950 && e.NewSize.Width < 1200)
            {
                GridViewScroller.Height = 690;
            }
            else if(e.NewSize.Width >= 1200 && e.NewSize.Width <= 1310)
            {
                GridViewScroller.Height = 715;
            }
            else if( e.NewSize.Width <= 1400)
            {
                GridViewScroller.Height = 780;
            }
            else if(e.NewSize.Width <= 1475)
            {
                GridViewScroller.Height = 730;
            }
        }

        public void UpdateSearchedUser()
        {
            UpdateSearchedUsers?.Invoke(_viewModel.MatchingUsers);
        }
    }
}