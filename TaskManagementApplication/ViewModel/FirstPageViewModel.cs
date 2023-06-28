using Microsoft.Toolkit.Collections;
using NSwag.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class FirstPageViewModel : FirstPageViewModelBase
    {
        private GetProjectsList _getProjectsList;
        //private GetUsersList _getUsersList; 
        
        public override void GetProjectsList()
        {
            _getProjectsList = new GetProjectsList(new PresenterGetProjectsList(this),new GetProjectListRequest(new CancellationTokenSource()));
            _getProjectsList.Execute();
        }

        //public override void GetUsersList(int projectId)
        //{
        //    _getUsersList = new GetUsersList(new GetUsersListRequest(projectId,new CancellationTokenSource()),new PresenterGetUsersList(this));
        //    _getUsersList.Execute();
        //}
    }


    public class PresenterGetProjectsList : IPresenterGetProjectsListCallback
    {
        public FirstPageViewModel firstPageViewModel;

        public PresenterGetProjectsList(FirstPageViewModel firstPage)
        {
            firstPageViewModel = firstPage;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<GetProjectListResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<GetProjectListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                PopulateData(response.Data.Projects);
            });
        }

        private void PopulateData(List<Project> data)
        {
            foreach (var p in data)
            {
                firstPageViewModel.ProjectsList.Add(p);
            }
        }
    }


    //public class PresenterGetUsersList : IPresenterGetUsersListCallback
    //{
    //    private FirstPageViewModel _firstPageViewModel;

    //    public PresenterGetUsersList(FirstPageViewModel firstPageViewModel)
    //    {
    //        _firstPageViewModel = firstPageViewModel;
    //    }

    //    public void OnError(BException errorMessage)
    //    {
            
    //    }

    //    public void OnFailure(ZResponse<GetUsersListResponse> response)
    //    {
            
    //    }

    //    public async void OnSuccessAsync(ZResponse<GetUsersListResponse> response)
    //    {
    //        await SwitchToMainUIThread.SwitchToMainThread(() =>
    //        {
    //            PopulateData(response.Data.AssignedUserList);
    //            //_firstPageViewModel.projectWithUsersList.Add();
    //        });
    //    }

    //    private void PopulateData(List<User> data)
    //    {
    //        //TODO : if no users,msg that no users were assigned yet
    //        foreach (var p in data)
    //        {
    //            _firstPageViewModel.UsersList.Add(p);               
    //        }
    //    }
    //}


    public abstract class FirstPageViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Project> ProjectsList = new ObservableCollection<Project>();
        //public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public ObservableCollection<ProjectWithUsersBO> projectWithUsersList = new ObservableCollection<ProjectWithUsersBO>();
        public ProjectWithUsersBO projectWithUsersBO = new ProjectWithUsersBO();
        //public abstract void GetUsersList(int projectId);
       
        public abstract void GetProjectsList();
        
    }

   

}
//need to do ProjectWithUsersBO
