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

        public override void GetUsersList(int projectId)
        {
            _getUsersList = new GetUsersList(new GetUsersListRequest(projectId, new CancellationTokenSource()), new PresenterGetUsersList(this));
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

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetUsersListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetUsersListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                PopulateData(response.Data.AssignedUserList);
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
        public abstract void GetUsersList(int projectId);


    }

}
