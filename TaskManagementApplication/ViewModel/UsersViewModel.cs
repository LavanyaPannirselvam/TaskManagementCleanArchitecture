using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain.Usecase;
using System.Collections.ObjectModel;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class UserViewModel : UserViewModelBase
    {
        private GetUsersList _getUsersList;

        public override void GetAllUsersList()
        {
            _getUsersList = new GetUsersList(new GetUsersListRequest(new CancellationTokenSource()), new PresenterGetUsersList(this));
            _getUsersList.Execute();
        }
    }

    public class PresenterGetUsersList : IPresenterGetUsersListCallback
    {
        private UserViewModelBase _usersViewModel;

        public PresenterGetUsersList(UserViewModelBase usersViewModel)
        {
            _usersViewModel = usersViewModel;
        }

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetUsersListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetUsersListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                PopulateData(response.Data.Data);
                //_usersViewModel.LoadUsers.LoadUsers(_usersViewModel.UsersList);
                //_firstPageViewModel.projectWithUsersList.Add();

            });
        }

        private void PopulateData(List<User> data)
        {
            //TODO : if no users,msg that no users were assigned yet
            foreach (var p in data)
            {
                _usersViewModel.UsersList.Add(p);
            }
        }
    }


    public abstract class UserViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public abstract void GetAllUsersList();
        public IDeleteUserPageUpdateNotification deleteUserPageUpdateNotification { get; set; }

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

        public void DeleteUser(string email)
        {
            DeleteUser _deleteUser;
            _deleteUser = new DeleteUser(new DeleteUserRequest(email,new CancellationTokenSource()),new PresenterDeleteUserCallback(this));
            _deleteUser.Execute();
        }
    }


    public interface IDeleteUserPageUpdateNotification
    {
        void NotificationMessage();
    }


    public class PresenterDeleteUserCallback : IPresenterDeleteUserCallback
    {
        UserViewModelBase _userViewModel;

        public PresenterDeleteUserCallback(UserViewModelBase model)
        {
            _userViewModel = model;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _userViewModel.ResponseString = response.Response.ToString();
                _userViewModel.deleteUserPageUpdateNotification.NotificationMessage();
            });
        }
    }

}
