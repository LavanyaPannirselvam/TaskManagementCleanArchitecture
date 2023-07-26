using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IAssignIssueToUserDataManager
    {
        void AssignIssueToUser(AssignIssueRequest request, IUsecaseCallbackBasecase<AssignIssueResponse> response);
    }


    public class AssignIssueRequest : IRequest
    {
        public int issueId;
        public int userId;
        public CancellationTokenSource CtsSource { get ; set ; }

        public AssignIssueRequest(int issueId, int userId, CancellationTokenSource ctsSource)
        {
            this.issueId = issueId;
            this.userId = userId;
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterAssignIssueCallback : IPresenterCallbackBasecase<AssignIssueResponse> { }


    public class AssignIssueToUser : UsecaseBase<AssignIssueResponse>
    {
        private IAssignIssueToUserDataManager _datamanager;
        private AssignIssueRequest _request;
        private IPresenterAssignIssueCallback _callback;

        public AssignIssueToUser(AssignIssueRequest request, IPresenterAssignIssueCallback callback)
        {
            _datamanager = ServiceProvider.GetInstance().Services.GetService<IAssignIssueToUserDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _datamanager.AssignIssueToUser(_request, new AssignIssueCallback(this));
        }


        public class AssignIssueCallback : IUsecaseCallbackBasecase<AssignIssueResponse>
        {
            private AssignIssueToUser _assignIssueToUser;

            public AssignIssueCallback(AssignIssueToUser assignIssueToUser)
            {
                _assignIssueToUser = assignIssueToUser;
            }

            public void OnResponseError(BaseException response)
            {
                _assignIssueToUser._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<AssignIssueResponse> response)
            {
                _assignIssueToUser._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<AssignIssueResponse> response)
            {
                _assignIssueToUser._callback.OnSuccessAsync(response);
            }
        }
    }


    public class AssignIssueResponse : ZResponse<ObservableCollection<User>>
    {
    }


    //public class AssignIssueResponse : ZResponse<List<User>>
    //{ }
}
