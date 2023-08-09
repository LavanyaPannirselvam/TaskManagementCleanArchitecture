using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeIssuePriorityDataManager
    {
        void ChangePriorityOfIssue(ChangeIssuePriorityRequest request, IUsecaseCallbackBasecase<ChangeIssuePriorityResponse> response);
    }


    public class ChangeIssuePriorityRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int issueId;
        public PriorityType priority;

        public ChangeIssuePriorityRequest(CancellationTokenSource ctsSource, int id, PriorityType priority)
        {
            CtsSource = ctsSource;
            this.issueId = id;
            this.priority = priority;
        }
    }


    public interface IPresenterChangeIssuePriorityCallback : IPresenterCallbackBasecase<ChangeIssuePriorityResponse>
    { }

    
    public class ChangeIssuePriority : UsecaseBase<ChangeIssuePriorityResponse>
    {
        private IChangeIssuePriorityDataManager _dataManager;
        private ChangeIssuePriorityRequest _request;
        private IPresenterChangeIssuePriorityCallback _response;

        public ChangeIssuePriority(ChangeIssuePriorityRequest request, IPresenterChangeIssuePriorityCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeIssuePriorityDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            this._dataManager.ChangePriorityOfIssue(_request, new ChangeIssuePriorityCallback(this));
        }

        public class ChangeIssuePriorityCallback : IUsecaseCallbackBasecase<ChangeIssuePriorityResponse>
        {
            private ChangeIssuePriority _projectPriority;

            public ChangeIssuePriorityCallback(ChangeIssuePriority projectPriority)
            {
                _projectPriority = projectPriority;
            }

            public void OnResponseError(BaseException response)
            {
                _projectPriority._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeIssuePriorityResponse> response)
            {
                _projectPriority._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeIssuePriorityResponse> response)
            {
                _projectPriority._response.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeIssuePriorityResponse : ZResponse<Issue>
    {
    }
}
