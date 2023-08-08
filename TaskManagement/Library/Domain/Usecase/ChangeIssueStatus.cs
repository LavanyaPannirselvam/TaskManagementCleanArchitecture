using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeIssueStatusDataManager
    {
        void ChangeStatusOfIssue(ChangeIssueStatusRequest request, IUsecaseCallbackBasecase<ChangeIssueStatusResponse> response);
    }


    public class ChangeIssueStatusRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int issueId;
        public StatusType status;

        public ChangeIssueStatusRequest(int id, StatusType type, CancellationTokenSource cts)
        {
            issueId = id;
            status = type;
            CtsSource = cts;
        }
    }


    public interface IPresenterChangeIssueStatusCallback : IPresenterCallbackBasecase<ChangeIssueStatusResponse>
    { }


    public class ChangeIssueStatus : UsecaseBase<ChangeIssueStatusResponse>
    {
        private IChangeIssueStatusDataManager _dataManager;
        private ChangeIssueStatusRequest _request;
        private IPresenterChangeIssueStatusCallback _response;

        public ChangeIssueStatus(ChangeIssueStatusRequest request, IPresenterChangeIssueStatusCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeIssueStatusDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            this._dataManager.ChangeStatusOfIssue(_request, new ChangeIssueStatusCallback(this));
        }


        public class ChangeIssueStatusCallback : IUsecaseCallbackBasecase<ChangeIssueStatusResponse>
        {
            private ChangeIssueStatus _projectStatus;

            public ChangeIssueStatusCallback(ChangeIssueStatus projectStatus)
            {
                _projectStatus = projectStatus;
            }

            public void OnResponseError(BaseException response)
            {
                _projectStatus._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeIssueStatusResponse> response)
            {
                _projectStatus._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeIssueStatusResponse> response)
            {
                _projectStatus._response.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeIssueStatusResponse : ZResponse<bool>
    { }
}
