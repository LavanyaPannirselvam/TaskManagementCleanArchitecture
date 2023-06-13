using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IRemoveProjectFromUserDataManager
    {
        void RemoveProjectFromUser(RemoveProjectRequest request,IUsecaseCallbackBasecase<RemoveProjectResponse> response);
    }


    public class RemoveProjectRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int projectId;
        public int userId;
        
        public RemoveProjectRequest(CancellationTokenSource ctsSource, int projectId, int userId)
        {
            CtsSource = ctsSource;
            this.projectId = projectId;
            this.userId = userId;
        }
    }


    public interface IPresenterRemoveProjectFromUserCallback : IPresenterCallbackBasecase<RemoveProjectResponse>
    { }


    public class RemoveProjectFromUser : UsecaseBase<RemoveProjectResponse>
    {
        private IRemoveProjectFromUserDataManager _datamanager;
        private RemoveProjectRequest _request;
        private IPresenterRemoveProjectFromUserCallback _response;
        
        public RemoveProjectFromUser(RemoveProjectRequest request, IPresenterRemoveProjectFromUserCallback response)
        {
            _datamanager = ServiceProvider.GetInstance().Services.GetService<IRemoveProjectFromUserDataManager>();
            _request = request;
            _response = response;
        }
        
        public override void Action()
        {
            this._datamanager.RemoveProjectFromUser(_request, new RemoveProjectCallback(this));
        }
        
        public class RemoveProjectCallback : IUsecaseCallbackBasecase<RemoveProjectResponse>
        {
            private RemoveProjectFromUser _projectRemovalFromUser;
            
            public RemoveProjectCallback(RemoveProjectFromUser projectRemovalFromUser)
            {
                _projectRemovalFromUser = projectRemovalFromUser;
            }

            public void OnResponseError(BException response)
            {
               _projectRemovalFromUser._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<RemoveProjectResponse> response)
            {
                _projectRemovalFromUser._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<RemoveProjectResponse> response)
            {
                _projectRemovalFromUser._response.OnSuccessAsync(response);
            }
        }
    }


    public class RemoveProjectResponse : ZResponse<string>
    {

    }
    
}
