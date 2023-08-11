using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.Converter;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Proximity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskManagementCleanArchitecture.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateNewUserPage : Page
    {
        CreateUserViewModelBase _createUser;
        Role userRole;
        public CreateNewUserPage()
        {
            this.InitializeComponent();
            _createUser = PresenterService.GetInstance().Services.GetService<CreateUserViewModelBase>();
            var roleList  = Enum.GetValues(typeof(Role)).Cast<Role>();
            UserRoleEnter.ItemsSource = roleList;
            UserRoleEnter.SelectedIndex = 0;
            ErrorMessage.Text = string.Empty;
            //UserNameEnter.Select(); // to Set Focus
            //UserNameEnter.Select(UserNameEnter.Text.Length, 0);
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameEnter.Text == string.Empty)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "User name cannot be empty";
            }
            else if (UserEmailEnter.Text == string.Empty)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "User email cannot be empty";
            }
            else
            {
                _createUser.CreateUser(UserNameEnter.Text, UserEmailEnter.Text, userRole);
            }
        }

        private void UserRoleEnter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = e.AddedItems[0].ToString();
            userRole = (Role)Enum.Parse(typeof(Role), selected.ToUpper().Replace(" ", ""));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CredentialsGrid.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CredentialsGrid.Visibility = Visibility.Collapsed;
            ErrorMessage.Text = string.Empty;
            UserRoleEnter.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;

        }
    }
}
