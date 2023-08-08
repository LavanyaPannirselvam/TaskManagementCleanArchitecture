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
using TaskManagementLibrary.Notifications;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class ProjectsViewModel : ProjectsViewModelBase
    {
        private GetProjectsList _getProjectsList;
        //private GetUsersList _getUsersList; 
        
        public override void GetProjectsList(string name,string email,int count,int skipCount)
        {
            _getProjectsList = new GetProjectsList(new PresenterGetProjectsList(this),new GetProjectListRequest(name,email,count,skipCount,new CancellationTokenSource()));
            _getProjectsList.Execute();
        }
    }


    public class PresenterGetProjectsList : IPresenterGetProjectsListCallback
    {
        public ProjectsViewModel projectPageViewModel;

        public PresenterGetProjectsList(ProjectsViewModel projectPage)
        {
            projectPageViewModel = projectPage;
        }

        public void OnError(BaseException errorMessage)
        {
        }

        public void OnFailure(ZResponse<GetProjectListResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<GetProjectListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if(response.Data.Data != null)
                {
                    PopulateData(response.Data.Data);
                    projectPageViewModel.DataGridVisibility = Visibility.Visible;
                    projectPageViewModel.TextVisibility = Visibility.Collapsed;
                }
                else
                {
                    projectPageViewModel.DataGridVisibility = Visibility.Collapsed;
                    projectPageViewModel.TextVisibility = Visibility.Visible;
                    projectPageViewModel.ResponseString =  response.Response;
                }
            });
        }

        private void PopulateData(List<Project> data)
        {
            foreach (var p in data)
            {
                projectPageViewModel.ProjectsList.Add(p);
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


    public abstract class ProjectsViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Project> ProjectsList = new ObservableCollection<Project>();
        public IProjectPageUpdate projectPageUpdate { get; set; }
        public abstract void GetProjectsList(string name,string email,int count,int skipCount);

        public void CreateProject(Project project)
        {
            CreateProject _createProject;
            _createProject = new CreateProject(new CreateProjectRequest(project, new CancellationTokenSource()), new PresenterCreateProjectCallback(this));
            _createProject.Execute();
        }

        private Visibility _textVisibility = Visibility.Collapsed;
        public Visibility TextVisibility
        {
            get { return _textVisibility; }
            set
            {
                _textVisibility = value;
                OnPropertyChanged(nameof(TextVisibility));
            }
        }

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

        private Visibility _dataGridVisibility = Visibility.Collapsed;
        public Visibility DataGridVisibility
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged(nameof(DataGridVisibility));
            }

        }
    }


    public interface IProjectPageUpdate
    {
        void ProjectUpdateNotification();
    }


    public class PresenterCreateProjectCallback : IPresenterCreateProjectCallback
    {
        private ProjectsViewModelBase _viewModel;
        public PresenterCreateProjectCallback(ProjectsViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<CreateProjectResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<CreateProjectResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                // _viewModel.NewProject = response.Data.NewProject;
                _viewModel.ResponseString = response.Response.ToString();
                _viewModel.projectPageUpdate.ProjectUpdateNotification();
                UIUpdation.OnProjectCreated(response.Data.Data);
                if (_viewModel.ProjectsList.Count != 0)
                {
                    _viewModel.DataGridVisibility = Visibility.Visible;
                    _viewModel.TextVisibility = Visibility.Collapsed;
                }
                else
                {
                    _viewModel.DataGridVisibility = Visibility.Collapsed;
                    _viewModel.TextVisibility = Visibility.Visible;
                    _viewModel.ResponseString = response.Response;
                }
            });
        }
    }
}
