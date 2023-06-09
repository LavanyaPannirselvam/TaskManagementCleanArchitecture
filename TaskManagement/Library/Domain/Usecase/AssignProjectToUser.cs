using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IAssignProjectToUserDataManager
    {
        void AssignProject(AssignProjectRequest request, IUsecaseCallbackBasecase<AssignProjectResponse> response);
    }


    public class AssignProjectRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int projectId;
        public int userId;
        
        public AssignProjectRequest(int projectId,int userId,CancellationTokenSource cts) 
        {
            this.projectId = projectId;
            this.userId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterAssignProjectCallback : IPresenterCallbackBasecase<AssignProjectResponse>
    { }


    public class AssignProjectToUser : UsecaseBase<AssignProjectResponse>
    {
        private IAssignProjectToUserDataManager _assignProjectDataManager;
        private AssignProjectRequest _request;
        private IPresenterAssignProjectCallback _response;
    
        public AssignProjectToUser(AssignProjectRequest request, IPresenterAssignProjectCallback response)
        {
            _assignProjectDataManager = ServiceProvider.GetInstance().Services.GetService<IAssignProjectToUserDataManager>();
            this._request = request;
            this._response = response;
        }
        
        public override void Action()
        {
            this._assignProjectDataManager.AssignProject(_request, new AssignProjectCallback(this));
        }


        public class AssignProjectCallback : IUsecaseCallbackBasecase<AssignProjectResponse>
        {
            private AssignProjectToUser _projectAssignment;
            
            public AssignProjectCallback(AssignProjectToUser obj)
            {
                _projectAssignment = obj;
            }

            public void OnResponseError(BException response)
            {
                _projectAssignment._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<AssignProjectResponse> response)
            {
                _projectAssignment._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<AssignProjectResponse> response)
            {
                _projectAssignment._response.OnSuccessAsync(response);
            }
        }
    }


    public class AssignProjectResponse : ZResponse<string>
    {
    }
    
}
