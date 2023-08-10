using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBManager;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeTaskPriorityDataManager
    {
        void ChangePriorityOfTask(ChangeTaskPriorityRequest request, IUsecaseCallbackBasecase<ChangeTaskPriorityResponse> response);
    }


    public class ChangeTaskPriorityRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int taskId;
        public PriorityType priority;
        
        public ChangeTaskPriorityRequest(CancellationTokenSource ctsSource, int id, PriorityType priority)
        {
            CtsSource = ctsSource;
            this.taskId = id;
            this.priority = priority;
        }
    }


    public interface IPresenterChangeTaskPriorityCallback : IPresenterCallbackBasecase<ChangeTaskPriorityResponse>
    { }


    public class ChangeTaskPriority : UsecaseBase<ChangeTaskPriorityResponse>
    {
        private IChangeTaskPriorityDataManager _dataManager;
        private ChangeTaskPriorityRequest _request;
        private IPresenterChangeTaskPriorityCallback _response;

        public ChangeTaskPriority(ChangeTaskPriorityRequest request, IPresenterChangeTaskPriorityCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeTaskPriorityDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            this._dataManager.ChangePriorityOfTask(_request,new ChangeTaskPriorityCallback(this));
        }
        
        public class ChangeTaskPriorityCallback : IUsecaseCallbackBasecase<ChangeTaskPriorityResponse>
        {
            private ChangeTaskPriority _projectPriority;
            
            public ChangeTaskPriorityCallback(ChangeTaskPriority projectPriority)
            {
                _projectPriority = projectPriority;
            }

            public void OnResponseError(BaseException response)
            {
                _projectPriority._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeTaskPriorityResponse> response)
            {
                _projectPriority._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeTaskPriorityResponse> response)
            {
                _projectPriority._response.OnSuccessAsync(response);
            }
        }
    }
    
    
    public class ChangeTaskPriorityResponse : ZResponse<Tasks>
    {
    }
}
