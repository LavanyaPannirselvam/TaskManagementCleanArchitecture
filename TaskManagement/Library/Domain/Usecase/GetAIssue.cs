using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetAIssueDataManager
    {
        void GetAIssue(GetAIssueRequest request, IUsecaseCallbackBasecase<GetAIssueResponse> response);
    }


    public class GetAIssueRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int issueId;

        public GetAIssueRequest(int id, CancellationTokenSource cancellationTokenSource)
        {
            issueId = id;
            CtsSource = cancellationTokenSource;
        }

    }

    public interface IPresenterGetAIssueListCallback : IPresenterCallbackBasecase<GetAIssueResponse> { };


    public class GetAIssue : UsecaseBase<GetAIssueResponse>
    {
        private IGetAIssueDataManager _dataManager;
        private GetAIssueRequest _request;
        private IPresenterGetAIssueListCallback _response;

        public GetAIssue(GetAIssueRequest request, IPresenterGetAIssueListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetAIssueDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetAIssue(_request, new GetAIssueCallback(this));
        }

        public class GetAIssueCallback : IUsecaseCallbackBasecase<GetAIssueResponse>
        {
            private GetAIssue _getAIssue;

            public GetAIssueCallback(GetAIssue getTasksList)
            {
                _getAIssue = getTasksList;
            }

            public void OnResponseError(BaseException response)
            {
                _getAIssue._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetAIssueResponse> response)
            {
                _getAIssue._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAIssueResponse> response)
            {
                _getAIssue._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetAIssueResponse : ZResponse<IssueBO>
    {
    }
    
}
