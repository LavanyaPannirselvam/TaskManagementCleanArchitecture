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
    public sealed partial class UserManagement : UserControl, IDeleteUserPageUpdateNotification
    {
        ObservableCollection<User> _matchingUsers;
        ObservableCollection<User> _toViewUsers;
        UserViewModelBase _viewModel;
        public static event Action<string> Notification;
        AppWindow appWindow;
        Frame appWindowContentFrame;
        public UserManagement()
        {
            this.InitializeComponent();
            _viewModel = PresenterService.GetInstance().Services.GetService<UserViewModelBase>();
            _viewModel.deleteUserPageUpdateNotification = this;
            _matchingUsers = new ObservableCollection<User>();
            _toViewUsers = new ObservableCollection<User>();
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
            _toViewUsers = _viewModel.UsersList;
        }

        private void NewUserCreated(User obj)
        {
            _viewModel.UsersList.Add(obj);
            _toViewUsers = _viewModel.UsersList;
        }

        private void ShowNotification(string msg)
        {
            //NotificationControl.Show(msg, 3000);
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
            SwitchTheme.AddUIRootElement(appWindowContentFrame);
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
            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var text = sender.Text.ToLower().Split(" ");
                if (text.Length > 0)
                {
                    UsersGridView.ItemsSource = _matchingUsers;
                    _matchingUsers.Clear();
                    _matchingUsers = CheckForUser(_viewModel.UsersList, text);
                    if (_matchingUsers.Count == 0)
                    {
                        //UsersGridView.ItemsSource = _viewModel.UsersList;
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
                else
                {
                    UsersGridView.Visibility = Visibility.Visible;
                    NoUsersText.Visibility = Visibility.Collapsed;
                    UsersGridView.ItemsSource = _viewModel.UsersList;
                }
            }
        }

        private ObservableCollection<User> CheckForUser(ObservableCollection<User> usersList,string[] input)
        {
            ObservableCollection<User> matchedUsers = new ObservableCollection<User>();
            foreach (var u in _viewModel.UsersList)
            {
                var found = input.All((key) =>
                {
                    return u.Name.ToLower().Contains(key);
                });
                if (found)
                {
                    matchedUsers.Add(u);
                }
            }
            if(matchedUsers.Count == 0) 
            {
                ObservableCollection<User> fromCollectionUsers = new ObservableCollection<User>();
                _viewModel.GetAllUsersList(9, _viewModel.UsersList.Count);
                fromCollectionUsers = _viewModel.UsersList;
                if(fromCollectionUsers.Count == 0) 
                {
                    return matchedUsers;
                }
                return CheckForUser(fromCollectionUsers, input);
            }
            return matchedUsers;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            _viewModel.GetAllUsersList(9,_viewModel.UsersList.Count);
        }
    }
}

//if(_matchingUsers.Count == 0) 
//{
//    //UsersGridView.ItemsSource = _viewModel.UsersList;
//    UsersGridView.Visibility = Visibility.Collapsed;
//    NoUsersText.Visibility = Visibility.Visible;
//}
//else
//{
//    UsersGridView.Visibility = Visibility.Visible;
//    NoUsersText.Visibility = Visibility.Collapsed;
//}
