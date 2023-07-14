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
    public interface ICreateIssueDataManager
    {
        void CreateIssue(CreateIssueRequest request, IUsecaseCallbackBasecase<CreateIssueResponse> response);
    }


    public class CreateIssueRequest : IRequest
    {
        public Issue issue;
        public CancellationTokenSource CtsSource { get ; set ; }

        public CreateIssueRequest(Issue issue, CancellationTokenSource ctsSource)
        {
            this.issue = issue;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterCreateIssueCallback : IPresenterCallbackBasecase<CreateIssueResponse>
    { }


    public class CreateIssue : UsecaseBase<CreateIssueResponse>
    {
        private ICreateIssueDataManager _createIssueDataManager;
        private IPresenterCreateIssueCallback _createIssueCallback;
        private CreateIssueRequest _createIssueRequest;

        public CreateIssue(CreateIssueRequest request ,IPresenterCreateIssueCallback callback)
        {
            _createIssueDataManager = ServiceProvider.GetInstance().Services.GetService<ICreateIssueDataManager>();
            _createIssueCallback = callback;
            _createIssueRequest = request;
        }

        public override void Action()
        {
            this._createIssueDataManager.CreateIssue(_createIssueRequest, new CreateIssueCallback(this));
        }

        public class CreateIssueCallback : IUsecaseCallbackBasecase<CreateIssueResponse>
        {
            private CreateIssue _createIssue;
           
            public CreateIssueCallback(CreateIssue createIssue)
            {
                _createIssue = createIssue;
            }

            public void OnResponseError(BException response)
            {
                _createIssue._createIssueCallback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<CreateIssueResponse> response)
            {
                _createIssue._createIssueCallback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<CreateIssueResponse> response)
            {
                _createIssue._createIssueCallback.OnSuccessAsync(response);
            }
        }
    }


    public class CreateIssueResponse : ZResponse<String>
    {
        public Issue NewIssue { get; set; }
    }

}
