using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
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
    public sealed partial class CreateUserPage : UserControl
    {
        CreateUserViewModelBase _createUser;
        Role userRole;
        public CreateUserPage()
        {
            this.InitializeComponent();
            _createUser = PresenterService.GetInstance().Services.GetService<CreateUserViewModelBase>();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            _createUser.CreateUser(UserNameEnter.Text, UserEmailEnter.Text,userRole);
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
    }
}
