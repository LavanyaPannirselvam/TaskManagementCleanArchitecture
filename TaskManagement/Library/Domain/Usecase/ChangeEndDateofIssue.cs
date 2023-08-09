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
    public interface IChangeEndDateofIssueDataManager
    {
        void ChangeEndDate(ChangeEndDateofIssueRequest request, IUsecaseCallbackBasecase<ChangeEndDateofIssueResponse> response);
    }

    public class ChangeEndDateofIssueRequest : IRequest
    {
        public int issueId;
        public DateTimeOffset date;
        public CancellationTokenSource CtsSource { get; set; }

        public ChangeEndDateofIssueRequest(int issueId, DateTimeOffset date, CancellationTokenSource ctsSource)
        {
            this.issueId = issueId;
            this.date = date;
            CtsSource = ctsSource;
        }
    }

    public interface IPresenterChangeEndDateOfIssueCallback : IPresenterCallbackBasecase<ChangeEndDateofIssueResponse> { }


    public class ChangeEndDateofIssue : UsecaseBase<ChangeEndDateofIssueResponse>
    {
        private IChangeEndDateofIssueDataManager _dataManager;
        private ChangeEndDateofIssueRequest _request;
        private IPresenterChangeEndDateOfIssueCallback _callback;

        public ChangeEndDateofIssue(ChangeEndDateofIssueRequest request, IPresenterChangeEndDateOfIssueCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeEndDateofIssueDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeEndDate(_request, new ChangeEndDateofIssueCallback(this));
        }


        public class ChangeEndDateofIssueCallback : IUsecaseCallbackBasecase<ChangeEndDateofIssueResponse>
        {
            private ChangeEndDateofIssue _changeEndDateofIssue;

            public ChangeEndDateofIssueCallback(ChangeEndDateofIssue obj)
            {
                _changeEndDateofIssue = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _changeEndDateofIssue._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeEndDateofIssueResponse> response)
            {
                _changeEndDateofIssue._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeEndDateofIssueResponse> response)
            {
                _changeEndDateofIssue._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeEndDateofIssueResponse : ZResponse<Issue>
    {

    }
}
