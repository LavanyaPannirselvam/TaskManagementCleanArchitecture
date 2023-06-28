using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;
using Windows.UI.Xaml;
using static TaskManagementLibrary.Models.User;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class LoginViewModel : LoginViewModelBase
    {
        Login login;
        public override void ValidateUser(string userId, string password)
        {
            login = new Login(new PresenterLoginCallback(this),new LoginRequest(new CancellationTokenSource(),userId, password));
            login.Execute();
        }
    }


    public class PresenterLoginCallback : IPresenterLoginCallback
    {
        private LoginViewModel loginViewModel;

        public PresenterLoginCallback(LoginViewModel loginViewModel)
        {
            this.loginViewModel = loginViewModel;
        }

        public async void OnError(BException errorMessage)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                loginViewModel.TextBoxVisibility = Visibility.Visible;
                loginViewModel.LoginResponseValue = errorMessage.exceptionMessage.ToString();
            });
        }

        public async void OnFailure(ZResponse<LoginResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                loginViewModel.TextBoxVisibility = Visibility.Visible;
                loginViewModel.LoginResponseValue = response.Response.ToString();
            });
        }

        public async void OnSuccessAsync(ZResponse<LoginResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                loginViewModel.CurrentUser = response.Data.currentUser;
                loginViewModel.LoginView?.UpdateLoginPage(loginViewModel.CurrentUser);
            });
        }
    }


    public abstract class LoginViewModelBase : NotifyPropertyBase
    {
        private string _response = String.Empty;
        public string LoginResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(LoginResponseValue));
            }
        }

        private Visibility _textBoxVisibility = Visibility.Collapsed;
        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged(nameof(TextBoxVisibility));

            }
        }

        private LoggedInUserBO _currentUser;
        public LoggedInUserBO CurrentUser
        {
            get { return this._currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public abstract void ValidateUser(string userId,string password);
        public ILoginView LoginView { get; set; }
    }


    public interface ILoginView
    {
        void UpdateLoginPage(LoggedInUserBO currentUser);
    }
}
//login view model's call back will have setuser method with current user -> loginViewModel
//set user will invoke the event -> login.xaml.cs
//even subscription will happen at main.xaml.cs

