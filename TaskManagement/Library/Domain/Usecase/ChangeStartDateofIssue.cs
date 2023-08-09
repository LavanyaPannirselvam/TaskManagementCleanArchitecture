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
    public interface IChangeStartDateofIssueDataManager
    {
        void ChangeStartDate(ChangeStartDateofIssueRequest request, IUsecaseCallbackBasecase<ChangeStartDateofIssueResponse> response);
    }

    public class ChangeStartDateofIssueRequest : IRequest
    {
        public int issueId;
        public DateTimeOffset date;
        public CancellationTokenSource CtsSource { get; set; }

        public ChangeStartDateofIssueRequest(int issueId, DateTimeOffset date, CancellationTokenSource ctsSource)
        {
            this.issueId = issueId;
            this.date = date;
            CtsSource = ctsSource;
        }
    }

    public interface IPresenterChangeStartDateOfIssueCallback : IPresenterCallbackBasecase<ChangeStartDateofIssueResponse> { }


    public class ChangeStartDateofIssue : UsecaseBase<ChangeStartDateofIssueResponse>
    {
        private IChangeStartDateofIssueDataManager _dataManager;
        private ChangeStartDateofIssueRequest _request;
        private IPresenterChangeStartDateOfIssueCallback _callback;

        public ChangeStartDateofIssue(ChangeStartDateofIssueRequest request, IPresenterChangeStartDateOfIssueCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeStartDateofIssueDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeStartDate(_request, new ChangeStartDateofIssueCallback(this));
        }


        public class ChangeStartDateofIssueCallback : IUsecaseCallbackBasecase<ChangeStartDateofIssueResponse>
        {
            private ChangeStartDateofIssue _changeStartDateofIssue;

            public ChangeStartDateofIssueCallback(ChangeStartDateofIssue obj)
            {
                _changeStartDateofIssue = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _changeStartDateofIssue._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeStartDateofIssueResponse> response)
            {
                _changeStartDateofIssue._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeStartDateofIssueResponse> response)
            {
                _changeStartDateofIssue._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeStartDateofIssueResponse : ZResponse<Issue>
    {

    }
}
