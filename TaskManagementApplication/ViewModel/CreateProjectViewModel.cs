using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskManagementCleanArchitecture.ViewModel.CreateProjectViewModel;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Domain.Usecase;
using System.Threading;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class CreateProjectViewModel : CreateProjectViewModelBase
    {
        CreateProject _createProject;
        public override void CreateProject(Project project)
        {
            _createProject = new CreateProject(new CreateProjectRequest(project, new CancellationTokenSource()), new PresenterCreateProjectCallback(this));
            _createProject.Execute();
        }
    }


    public class PresenterCreateProjectCallback : IPresenterCreateProjectCallback
    {
        private CreateProjectViewModel _viewModel;
        public PresenterCreateProjectCallback(CreateProjectViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BException errorMessage)
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
                    _viewModel.NewProject = response.Data.NewProject;
                    // _viewModel.AddedView.UpdateNewProject(_viewModel.NewProject);
                });
        }
    }



    public abstract class CreateProjectViewModelBase : NotifyPropertyBase
    {
        public abstract void CreateProject(Project project);
        private Project _newProject;
        public Project NewProject
        {
            get { return this._newProject; }
            set
            {
                _newProject = value;
                OnPropertyChanged(nameof(NewProject));
            }
        }

        public IProjectAddedView AddedView { get; set; }
    }

    public interface IProjectAddedView
    {
        void UpdateNewProject(Project newProject);
    }

}

