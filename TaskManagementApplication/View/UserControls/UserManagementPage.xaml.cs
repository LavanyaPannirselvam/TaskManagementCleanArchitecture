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
    public sealed partial class DeleteUserPage : UserControl , IDeleteUserPageUpdateNotification
    {
        ObservableCollection<User> _users;
        UserViewModelBase _viewModel;
        public static event Action<string> Notification;
        public DeleteUserPage()
        {
            this.InitializeComponent();
            _viewModel = PresenterService.GetInstance().Services.GetService<UserViewModelBase>();
            _viewModel.deleteUserPageUpdateNotification = this;
            _viewModel.UsersList.Clear();
        }

        public void NotificationMessage()
        {
            Notification.Invoke(_viewModel.ResponseString);
        }

        //private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
        //    _users = _viewModel.UsersList;
        //    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        //    {
        //        var suitableItems = new ObservableCollection<string>();
        //       var splitText = sender.Text.ToLower().Split(" ");
        //        foreach (var user in _users)
        //        {
        //            var found = splitText.All((key) =>
        //            {
        //                return user.Email.ToLower().Contains(key);
        //            });
        //            if (found)
        //            {
        //                suitableItems.Add(user.Email);
        //            }
        //        }
        //        if (suitableItems.Count == 0)
        //        {
        //            suitableItems.Add("No results found");
        //        }
        //        if (sender.Text != string.Empty)
        //            sender.ItemsSource = suitableItems;
        //        else sender.ItemsSource = null;
        //        _viewModel.UsersList.Clear();
        //        _viewModel.GetAllUsersList();
        //    }
        //}


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllUsersList();
            Notification += ShowNotification;
            UIUpdation.NewUserCreated += NewUserCreated;
            UIUpdation.UserDeleted += UIUpdation_UserDeleted;
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
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame appWindowContentFrame = new Frame();
            var newUserPage = new CreateNewUserPage();
            appWindowContentFrame.Navigate(newUserPage.GetType());
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            SwitchTheme.AddUIRootElement(appWindowContentFrame);
            await appWindow.TryShowAsync();
            appWindow.Closed += delegate
            {
                appWindowContentFrame.Content = null;
                appWindow = null;
            };
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var user = ((FrameworkElement)sender).DataContext as User;
            _viewModel.DeleteUser(user.Email);
        }
    }
}
