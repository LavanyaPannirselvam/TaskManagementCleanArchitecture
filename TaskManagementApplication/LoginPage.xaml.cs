using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LoginPage : Page , ILoginView
    {
        private LoginViewModelBase _loginBaseViewModel;
        public static event Action<LoggedInUserBO> OnLoginSuccess;
        public LoginPage()
        {
            this.InitializeComponent();
            _loginBaseViewModel = PresenterService.GetInstance().Services.GetService<LoginViewModelBase>();
            _loginBaseViewModel.LoginView = this;
            _loginBaseViewModel.LoginResponseValue = string.Empty;
        }

        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                Password.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                Password.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (UserId.Text == "")
            {
                _loginBaseViewModel.TextBoxVisibility = Visibility.Visible;
                ResultText.Text = "Enter User ID";
            }
            else
            {
                _loginBaseViewModel.ValidateUser(UserId.Text, Password.Password);
                UserId.Text = ""; 
                Password.Password = "";
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            _loginBaseViewModel.TextBoxVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password.Length >= 5 && password.Length <= 10 && password.Any(char.IsLower) && password.Any(char.IsUpper) && (!password.Contains(" ")) && CheckForSpecialCharacter(password))
            {
                SubmitLoginDetails.IsEnabled = true;
            }
            else
            {
                SubmitLoginDetails.IsEnabled = false;
            }
        }

        private bool CheckForSpecialCharacter(string passwd)
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (passwd.Contains(ch))
                    return true;
            }
            return false;
        }

        public void UpdateLoginPage(LoggedInUserBO user)
        {
            OnLoginSuccess.Invoke(user);
        }

        
    }
}
