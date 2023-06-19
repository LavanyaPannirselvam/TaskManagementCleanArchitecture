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

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class LoginViewModel : LoginViewModelBase
    {
        Login login;
        public string userId;
        public string password;
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
                loginViewModel.loginView.UpdateLoginPage(loginViewModel.CurrentUser);
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

        private User _currentUser;
        public User CurrentUser
        {
            get { return this._currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(_currentUser));
            }
        }

        private string _resetPasswordResponseValue;
        public string ResetPasswordResponseValue
        {
            get { return this._resetPasswordResponseValue; }
            set
            {
                _resetPasswordResponseValue = value;
                OnPropertyChanged(nameof(ResetPasswordResponseValue));
            }
        }

        private bool _redirect = true;
        public bool Redirect
        {
            get { return this._redirect; }
            set
            {
                _redirect = value;
                OnPropertyChanged(nameof(Redirect));
            }
        }
        public abstract void ValidateUser(string userId,string password);
        public ILoginView loginView { get; set; }
    }


    public interface ILoginView
    {
        void UpdateLoginPage(User currentUser);
    }
}

