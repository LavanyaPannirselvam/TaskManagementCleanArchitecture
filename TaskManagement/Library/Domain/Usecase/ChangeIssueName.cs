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
   
    public interface IChangeIssueNameDataManager
    {
        void ChangeName(ChangeIssueNameRequest request, IUsecaseCallbackBasecase<ChangeIssueNameResponse> reqponse);
    }
    

    public class ChangeIssueNameRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public string name;
        public int issueId;

        public ChangeIssueNameRequest(string name, int issueId, CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
            this.name = name;
            this.issueId = issueId;
        }
    }


    public interface IPresenterChangeIssueNameCallback : IPresenterCallbackBasecase<ChangeIssueNameResponse>
    { }


    public class ChangeIssueName : UsecaseBase<ChangeIssueNameResponse>
    {
        private IChangeIssueNameDataManager _dataManager;
        private ChangeIssueNameRequest _request;
        private IPresenterChangeIssueNameCallback _callback;

        public ChangeIssueName(ChangeIssueNameRequest request, IPresenterChangeIssueNameCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeIssueNameDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeName(_request, new PresenterChangeIssueNameCallback(this));
        }


        public class PresenterChangeIssueNameCallback : IUsecaseCallbackBasecase<ChangeIssueNameResponse>
        {
            private ChangeIssueName changeIssueName;

            public PresenterChangeIssueNameCallback(ChangeIssueName changeIssueName)
            {
                this.changeIssueName = changeIssueName;
            }

            public void OnResponseError(BaseException response)
            {
                changeIssueName._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeIssueNameResponse> response)
            {
                changeIssueName._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeIssueNameResponse> response)
            {
                changeIssueName._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeIssueNameResponse : ZResponse<Issue>
    { }
}
