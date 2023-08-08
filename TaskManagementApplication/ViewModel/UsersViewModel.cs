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
using TaskManagementLibrary.Notifications;
using Windows.UI.Xaml.Data;
using Windows.Foundation;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class UserViewModel : UserViewModelBase
    {
        private GetUsersList _getUsersList;

        public override void GetAllUsersList(int count,int skipCount)
        {
            _getUsersList = new GetUsersList(new GetUsersListRequest(count,skipCount,new CancellationTokenSource()), new PresenterGetUsersList(this));
            _getUsersList.Execute();
        }
    }

    public class PresenterGetUsersList : IPresenterGetUsersListCallback
    {
        private UserViewModelBase _usersViewModel;
        private int _itemsPerPage;
        private bool _hasMoreItems;
        private int _currentPage;

        public PresenterGetUsersList(UserViewModelBase usersViewModel,int itemsPerpage = 10)
        {
            _usersViewModel = usersViewModel;
            _itemsPerPage = itemsPerpage;
            _hasMoreItems = true;
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
            });
        }

        private void PopulateData(List<User> data)
        {
            foreach (var p in data)
            {
                _usersViewModel.UsersList.Add(p);
            }
        }
    }


    public abstract class UserViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public abstract void GetAllUsersList(int count,int skipCount);
        public IDeleteUserPageUpdateNotification deleteUserPageUpdateNotification { get; set; }
        public IUpdateSearchedUser updateSearchedUser { get; set; }
        
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

        private ObservableCollection<User> _matchingUsers = new ObservableCollection<User>();
        public ObservableCollection<User> MatchingUsers
        {
            get { return _matchingUsers; }
            set
            {
                _matchingUsers = value;
                OnPropertyChanged(nameof(MatchingUsers));
            }
        }

        public void DeleteUser(string email)
        {
            DeleteUser _deleteUser;
            _deleteUser = new DeleteUser(new DeleteUserRequest(email,new CancellationTokenSource()),new PresenterDeleteUserCallback(this));
            _deleteUser.Execute();
        }

        public void GetMatchingUsers(string input)
        {
            GetAllMatchingUsers _getAllUsers;
            _getAllUsers = new GetAllMatchingUsers(new GetAllMatchingUsersRequest(input, new CancellationTokenSource()), new PresenterAllMatchingUsersCallback(this));
            _getAllUsers.Execute();
        }
    }


    public interface IDeleteUserPageUpdateNotification
    {
        void NotificationMessage();
    }


    public interface IUpdateSearchedUser
    {
        void UpdateSearchedUser();
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

        public void OnFailure(ZResponse<DeleteUserResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<DeleteUserResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _userViewModel.ResponseString = response.Response.ToString();
                _userViewModel.deleteUserPageUpdateNotification.NotificationMessage();
                UIUpdation.OnUserDeleted(response.Data.Data);
            });
        }
    }


    public class PresenterAllMatchingUsersCallback : IPresenterGetAllMatchingUsersCallback
    {
        private UserViewModelBase _getMatchingUsers;

        public PresenterAllMatchingUsersCallback(UserViewModelBase obj)
        {
            _getMatchingUsers = obj;
        }
        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<GetAllMatchingUsersResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<GetAllMatchingUsersResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _getMatchingUsers.MatchingUsers = response.Data.Data;
                _getMatchingUsers.updateSearchedUser.UpdateSearchedUser();
            });
        }
    }

}
