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

        public void OnError(BException errorMessage)
        {
            userViewModel.ResponseString = errorMessage.Message;
            userViewModel.ResponseStringVisibility = Visibility.Visible;
        }

        public void OnFailure(ZResponse<AddUserResponse> response)
        {
            userViewModel.ResponseString = response.Response.ToString();
            userViewModel.ResponseStringVisibility = Visibility.Visible;
        }

        public void OnSuccessAsync(ZResponse<AddUserResponse> response)
        {
            throw new NotImplementedException();
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
    }
}
