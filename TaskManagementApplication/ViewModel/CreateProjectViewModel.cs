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
        CreateProject createProject;
        public override void CreateProject(Project project)
        {
            createProject = new CreateProject(new CreateProjectRequest(project, new CancellationTokenSource()), new PresenterCreateProjectCallback(this));
            createProject.Execute();
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

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
                {

                });
        }
    }



    public abstract class CreateProjectViewModelBase : NotifyPropertyBase
    {
        public abstract void CreateProject(Project project);
        private string _response = String.Empty;
        public string Response
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }
    }

}

