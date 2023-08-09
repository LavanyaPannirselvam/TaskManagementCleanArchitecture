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
    public interface IChangeIssueDescriptionDataManager
    {
        void ChangeDescription(ChangeIssueDescriptionRequest request, IUsecaseCallbackBasecase<ChangeIssueDescriptionResponse> reqponse);
    }


    public class ChangeIssueDescriptionRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public string description;
        public int issueId;

        public ChangeIssueDescriptionRequest(string description, int issueId,CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
            this.description = description;
            this.issueId = issueId;
        }
    }


    public interface IPresenterChangeIssueDescriptionCallback : IPresenterCallbackBasecase<ChangeIssueDescriptionResponse>
    {}


    public class ChangeIssueDescription : UsecaseBase<ChangeIssueDescriptionResponse>
    {
        private IChangeIssueDescriptionDataManager _dataManager;
        private ChangeIssueDescriptionRequest _request;
        private IPresenterChangeIssueDescriptionCallback _callback;

        public ChangeIssueDescription(ChangeIssueDescriptionRequest request, IPresenterChangeIssueDescriptionCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeIssueDescriptionDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeDescription(_request, new PresenterChangeIssueDescriptionCallback(this));
        }


        public class PresenterChangeIssueDescriptionCallback : IUsecaseCallbackBasecase<ChangeIssueDescriptionResponse>
        {
            private ChangeIssueDescription changeIssueName;

            public PresenterChangeIssueDescriptionCallback(ChangeIssueDescription changeIssueName)
            {
                this.changeIssueName = changeIssueName;
            }

            public void OnResponseError(BaseException response)
            {
                changeIssueName._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeIssueDescriptionResponse> response)
            {
                changeIssueName._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeIssueDescriptionResponse> response)
            {
                changeIssueName._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeIssueDescriptionResponse : ZResponse<Issue>
    { }
}
