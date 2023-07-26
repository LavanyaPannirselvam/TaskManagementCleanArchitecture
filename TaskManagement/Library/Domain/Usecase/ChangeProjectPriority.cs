using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBManager;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeProjectPriorityDataManager
    {
        void ChangePriorityOfProject(ChangeProjectPriorityRequest request, IUsecaseCallbackBasecase<ChangeProjectPriorityResponse> response);
    }


    public class ChangeProjectPriorityRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int projectId;
        public PriorityType priority;
        
        public ChangeProjectPriorityRequest(CancellationTokenSource ctsSource, int projectId, PriorityType priority)
        {
            CtsSource = ctsSource;
            this.projectId = projectId;
            this.priority = priority;
        }
    }


    public interface IPresenterChangeProjectPriorityCallback : IPresenterCallbackBasecase<ChangeProjectPriorityResponse>
    { }


    public class ChangeProjectPriority : UsecaseBase<ChangeProjectPriorityResponse>
    {
        private IChangeProjectPriorityDataManager _dataManager;
        private ChangeProjectPriorityRequest _request;
        private IPresenterChangeProjectPriorityCallback _response;
        
        public ChangeProjectPriority(ChangeProjectPriorityRequest request, IPresenterChangeProjectPriorityCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeProjectPriorityDataManager>();
            _request = request;
            _response = response;
        }
        
        public override void Action()
        {
            this._dataManager.ChangePriorityOfProject(_request,new ChangeProjectPriorityCallback(this));
        }
        
        public class ChangeProjectPriorityCallback : IUsecaseCallbackBasecase<ChangeProjectPriorityResponse>
        {
            private ChangeProjectPriority _projectPriority;
            
            public ChangeProjectPriorityCallback(ChangeProjectPriority projectPriority)
            {
                _projectPriority = projectPriority;
            }

            public void OnResponseError(BaseException response)
            {
                _projectPriority._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeProjectPriorityResponse> response)
            {
                _projectPriority._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeProjectPriorityResponse> response)
            {
                _projectPriority._response.OnSuccessAsync(response);
            }
        }
    }
    
    
    public class ChangeProjectPriorityResponse : ZResponse<string>
    {
        //if just string is being returned back,use bool in <> of ZResponse
    }
}
