using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    internal class CreateUserViewModel : CreateUserViewModelBase
    {
        public CreateUserAccount userAccount;
        public override void CreateUser(string name, string email, Role role)
        {
            userAccount = new CreateUserAccount(new PresenterCreateUserCallback(this),new CreateUserRequest(name, email, role , new CancellationTokenSource()));
            userAccount.Execute();
        }
    }

    public class PresenterCreateUserCallback : IPresenterCreateUserCallback
    {
        public CreateUserViewModelBase userViewModel;
        public PresenterCreateUserCallback(CreateUserViewModelBase userViewModel)
        {
            this.userViewModel = userViewModel;
        }

        public async void OnError(BaseException errorMessage)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                userViewModel.ResponseString = errorMessage.exceptionMessage.ToString();
                userViewModel.ResponseStringVisibility = Visibility.Visible;
            });
        }

        public async void OnFailure(ZResponse<AddUserResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                userViewModel.ResponseString = response.Response.ToString();
                userViewModel.ResponseStringVisibility = Visibility.Visible;
            });
        }

        public async void OnSuccessAsync(ZResponse<AddUserResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                userViewModel.CredentialsGridVisibility = Visibility.Visible;
                userViewModel.UserId = response.Data.Data.Email.ToString();
                userViewModel.Password = response.Data.Data.Password.ToString();
            });
        }
    }


    public abstract class CreateUserViewModelBase : NotifyPropertyBase
    {
        public abstract void CreateUser(string name, string email, Role role);
        public User NewUser { get; set; }

        private string _responseString = string.Empty;
        public string ResponseString
        {
            get { return _responseString; }
            set 
            { 
                _responseString = value; 
                OnPropertyChanged(nameof(ResponseString));
            }
        }

        private Visibility _responseStringVisibility = Visibility.Collapsed;
        public Visibility ResponseStringVisibility
        {
            get { return _responseStringVisibility; }
            set
            {
                _responseStringVisibility = value;
                OnPropertyChanged(nameof(ResponseStringVisibility));
            }
        }

        private Visibility _credentialsGridVisibility = Visibility.Collapsed;
        public Visibility CredentialsGridVisibility
        {
            get { return _credentialsGridVisibility; }
            set
            {
                _credentialsGridVisibility = value;
                OnPropertyChanged(nameof(CredentialsGridVisibility));
            }
        }

        private string _userId = string.Empty;
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }
}
