using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using EnumConverter = TaskManagementCleanArchitecture.Converter.EnumConverter;

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
        EnumConverter _enumConverter;
        public CreateNewUserPage()
        {
            this.InitializeComponent();
            _createUser = PresenterService.GetInstance().Services.GetService<CreateUserViewModelBase>();
            UserRoleEnter.ItemsSource = EnumConverter.EnumToStringConverter(typeof(Role));
            UserRoleEnter.SelectedIndex = 0;
            ErrorMessage.Text = string.Empty;
            _enumConverter = new EnumConverter();
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
                ErrorMessage.Text = string.Empty;
                _createUser.CreateUser(UserNameEnter.Text, UserEmailEnter.Text, userRole);
            }
        }

        private void UserRoleEnter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userRole = (Role)_enumConverter.ConvertBack(e.AddedItems[0], typeof(Role), null, null);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CredentialsGrid.Visibility = Visibility.Collapsed;
            UserNameEnter.Text = string.Empty;
            UserEmailEnter.Text = string.Empty;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CredentialsGrid.Visibility = Visibility.Collapsed;
            ErrorMessage.Text = string.Empty;
            UserRoleEnter.SelectionChanged += UserRoleEnter_SelectionChanged;
        }
    }
}
