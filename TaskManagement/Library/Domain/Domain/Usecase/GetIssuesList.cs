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
    public interface IGetIssuesListDataManager
    {
        void GetIssues(GetIssuesListRequest request, IUsecaseCallbackBasecase<GetIssuesListResponse> response);
    }


    public class GetIssuesListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int projectId;

        public GetIssuesListRequest(int projectId, CancellationTokenSource cancellationTokenSource)
        {
            this.projectId = projectId;
            CtsSource = cancellationTokenSource;
        }
    }

    public interface IPresenterGetIssuesListCallback : IPresenterCallbackBasecase<GetIssuesListResponse> { };


    public class GetIssuesList : UsecaseBase<GetIssuesListResponse>
    {
        private IGetIssuesListDataManager _dataManager;
        private GetIssuesListRequest _request;
        private IPresenterGetIssuesListCallback _response;

        public GetIssuesList(GetIssuesListRequest request, IPresenterGetIssuesListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetIssuesListDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetIssues(_request, new GetIssuesListCallback(this));
        }

        public class GetIssuesListCallback : IUsecaseCallbackBasecase<GetIssuesListResponse>
        {
            private GetIssuesList _getIssuesList;

            public GetIssuesListCallback(GetIssuesList getTasksList)
            {
                _getIssuesList = getTasksList;
            }

            public void OnResponseError(BException response)
            {
                _getIssuesList._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetIssuesListResponse> response)
            {
                _getIssuesList._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetIssuesListResponse> response)
            {
                _getIssuesList._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetIssuesListResponse : ZResponse<List<Issue>>
    {
        public List<Issue> Issues;
    }
}
