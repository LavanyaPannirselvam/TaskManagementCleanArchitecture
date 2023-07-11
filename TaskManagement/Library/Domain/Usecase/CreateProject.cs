
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface ICreateProjectDataManager
    {
        void CreateProject(CreateProjectRequest request, IUsecaseCallbackBasecase<CreateProjectResponse> response);
    }


    public class CreateProjectRequest : IRequest
    {
        public Project project { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public CreateProjectRequest(Project project, CancellationTokenSource Source)
        {
            this.project = project;
            CtsSource = Source;
        }
    }


    public interface IPresenterCreateProjectCallback : IPresenterCallbackBasecase<CreateProjectResponse>
    { }
    

    public class CreateProject : UsecaseBase<CreateProjectResponse>
    {
        private ICreateProjectDataManager _createProjectDataManager;
        private CreateProjectRequest _createProjectRequest;
        private IPresenterCreateProjectCallback _presenterCreateProjectResponse;
        
        public CreateProject(CreateProjectRequest request, IPresenterCreateProjectCallback response)
        {
            _createProjectDataManager = ServiceProvider.GetInstance().Services.GetService<ICreateProjectDataManager>(); ;
            _createProjectRequest = request;
            _presenterCreateProjectResponse = response;
        }
        
        public override void Action()
        {
            this._createProjectDataManager.CreateProject(_createProjectRequest, new CreateProjectCallback(this));
        }
        
        
        public class CreateProjectCallback : IUsecaseCallbackBasecase<CreateProjectResponse>
        {
            private CreateProject _createProject;
            
            public CreateProjectCallback(CreateProject createProject)
            {
                _createProject = createProject;
            }
            
            public void OnResponseError(BException error)
            {
                _createProject._presenterCreateProjectResponse?.OnError(error);
            }
            
            public void OnResponseFailure(ZResponse<CreateProjectResponse> response)
            {
                _createProject._presenterCreateProjectResponse?.OnFailure(response);
            }
            
            public void OnResponseSuccess(ZResponse<CreateProjectResponse> response)
            {
                _createProject._presenterCreateProjectResponse?.OnSuccessAsync(response);
            }
        }
    }
    
    
    public class CreateProjectResponse : ZResponse<Project>
    {
        public Project NewProject { get; set; }
    }
}

