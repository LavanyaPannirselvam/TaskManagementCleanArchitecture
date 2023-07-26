using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            _users = _viewModel.UsersList;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new ObservableCollection<string>();
               var splitText = sender.Text.ToLower().Split(" ");
                foreach (var user in _users)
                {
                    var found = splitText.All((key) =>
                    {
                        return user.Email.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(user.Email);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                if (sender.Text != string.Empty)
                    sender.ItemsSource = suitableItems;
                else sender.ItemsSource = null;
                _viewModel.UsersList.Clear();
                _viewModel.GetAllUsersList();
            }
        }

        private void DeleteUserBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var result = args.ChosenSuggestion;
            foreach (var user in _users)
            {
                if (user.Email.Equals(result))
                    _viewModel.DeleteUser(user.Email);
            }
            DeleteUser.Text = string.Empty;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllUsersList();
            Notification += ShowNotification;
        }

        private void ShowNotification(string msg)
        {
            NotificationControl.Show(msg, 3000);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Notification -= ShowNotification;
        }
    }
}
